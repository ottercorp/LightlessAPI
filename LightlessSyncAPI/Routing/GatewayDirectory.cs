using System.Collections.Frozen;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LightlessSync.API.Routing
{
    [Flags]
    public enum GatewayCapabilities
    {
        None = 0,
        Authentication = 1,
        Hub = 2,
        Profiles = 4,
        Files = 8,
        All = Authentication | Hub | Profiles | Files,
    }

    public sealed record GatewayEndpoint(string Id, string Name, Uri BaseUri, int Priority = 0, bool Enabled = true,
        [property: JsonRequired] GatewayCapabilities Capabilities = GatewayCapabilities.All);

    public sealed record GatewayManifest(int Version, long Revision, Uri ServiceUri,
        DateTimeOffset IssuedAt, DateTimeOffset ExpiresAt, GatewayEndpoint[] Gateways)
    {
        public bool IsCurrent(DateTimeOffset now)
        {
            return IssuedAt <= now.AddMinutes(5) && ExpiresAt > now;
        }
    }

    /// <summary> the P-256 signature
    public sealed record SignedGatewayManifest(string KeyId, string Payload, string Signature);

    public sealed record GatewayStatusDto(int Version, Uri ServiceUri, string GatewayId,
        GatewayCapabilities Ready, DateTimeOffset CheckedAt);

    public static class GatewayDirectory
    {
        public const int Version = 1;
        public const int MaximumGateways = 16; // temporary?
        public const int MaximumDocumentBytes = 128 * 1024;
        public const string StatusPath = "/gateway/status";
        public const string ProbePrefix = "/gateway/probe/";
        public const string HttpClientKey = "LightlessService";

        public static FrozenDictionary<string, GatewayCapabilities> Roles { get; } =
            new Dictionary<string, GatewayCapabilities>(StringComparer.Ordinal)
            {
                ["auth"] = GatewayCapabilities.Authentication,
                ["hub"] = GatewayCapabilities.Hub,
                ["profiles"] = GatewayCapabilities.Profiles,
                ["files"] = GatewayCapabilities.Files,
            }.ToFrozenDictionary(StringComparer.Ordinal);

        public static JsonSerializerOptions JsonOptions { get; } = new(JsonSerializerDefaults.Web)
        {
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            MaxDepth = 16,
        };

        public static GatewayManifest Verify(ReadOnlySpan<byte> document,
            IReadOnlyDictionary<string, string> publicKeys, Uri serviceUri)
        {
            if (document.IsEmpty || document.Length > MaximumDocumentBytes)
                throw new InvalidDataException("Gateway directory size is invalid");

            SignedGatewayManifest envelope = JsonSerializer.Deserialize<SignedGatewayManifest>(document, JsonOptions)
                ?? throw new InvalidDataException("Gateway directory is empty");
            if (string.IsNullOrEmpty(envelope.KeyId)
                || !publicKeys.TryGetValue(envelope.KeyId, out string? publicKey)
                || string.IsNullOrEmpty(envelope.Payload) || string.IsNullOrEmpty(envelope.Signature))
                throw new InvalidDataException("Gateway directory signing key is not trusted");

            byte[] payload = Convert.FromBase64String(envelope.Payload);
            byte[] signature = Convert.FromBase64String(envelope.Signature);
            using ECDsa signer = ECDsa.Create();
            signer.ImportFromPem(publicKey);
            if (!IsP256(signer) || signature.Length != 64
                || !signer.VerifyData(payload, signature, HashAlgorithmName.SHA256,
                    DSASignatureFormat.IeeeP1363FixedFieldConcatenation))
                throw new InvalidDataException("Gateway directory signature is invalid");

            GatewayManifest manifest = JsonSerializer.Deserialize<GatewayManifest>(payload, JsonOptions)
                ?? throw new InvalidDataException("Gateway directory payload is empty");
            Validate(manifest, serviceUri);
            return manifest;
        }

        public static byte[] Sign(GatewayManifest manifest, string keyId, ECDsa privateKey)
        {
            Validate(manifest, manifest.ServiceUri);
            if (string.IsNullOrWhiteSpace(keyId) || !IsP256(privateKey))
                throw new InvalidDataException("Gateway directory signing key is invalid");

            byte[] payload = JsonSerializer.SerializeToUtf8Bytes(manifest, JsonOptions);
            byte[] signature = privateKey.SignData(payload, HashAlgorithmName.SHA256,
                DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
            byte[] document = JsonSerializer.SerializeToUtf8Bytes(new SignedGatewayManifest(keyId,
                Convert.ToBase64String(payload), Convert.ToBase64String(signature)), JsonOptions);
            if (document.Length > MaximumDocumentBytes)
                throw new InvalidDataException("Gateway directory exceeds its size limit");
            return document;
        }

        public static void Validate(GatewayManifest manifest, Uri serviceUri)
        {
            if (manifest.Version != Version || manifest.Revision <= 0
                || !GatewayUri.TryNormalize(serviceUri, out Uri expectedService)
                || !GatewayUri.TryNormalize(manifest.ServiceUri, out Uri actualService)
                || expectedService != actualService)
                throw new InvalidDataException("Gateway directory service or version is invalid");
            if (manifest.ExpiresAt <= manifest.IssuedAt
                || manifest.ExpiresAt - manifest.IssuedAt > TimeSpan.FromDays(90))
                throw new InvalidDataException("Gateway directory validity period is invalid");
            ValidateEndpoints(expectedService, manifest.Gateways);
        }

        public static void ValidateEndpoints(Uri serviceUri, GatewayEndpoint[]? gateways)
        {
            if (!GatewayUri.TryNormalize(serviceUri, out Uri expectedService))
                throw new InvalidDataException("Gateway service origin is invalid");
            if (gateways is null || gateways.Length > MaximumGateways)
                throw new InvalidDataException("Gateway directory count is invalid");

            HashSet<string> ids = new(StringComparer.Ordinal);
            HashSet<Uri> origins = [];
            foreach (GatewayEndpoint gateway in gateways)
            {
                if (gateway is null || string.IsNullOrWhiteSpace(gateway.Id)
                    || gateway.Id.Length > 64 || gateway.Id == "direct"
                    || string.IsNullOrWhiteSpace(gateway.Name) || gateway.Name.Length > 128
                    || gateway.Priority is < 0 or > 1000 || (gateway.Capabilities & ~GatewayCapabilities.All) != 0
                    || !ids.Add(gateway.Id)
                    || !GatewayUri.TryNormalize(gateway.BaseUri, out Uri origin)
                    || origin == expectedService || !origins.Add(origin))
                    throw new InvalidDataException("Gateway directory entry is invalid");
            }
        }

        public static bool IsP256(ECDsa key)
        {
            return key.ExportParameters(false).Curve.Oid.Value == ECCurve.NamedCurves.nistP256.Oid.Value;
        }
    }

    public static class GatewayUri
    {
        public static bool TryNormalize(Uri? value, out Uri normalized)
        {
            normalized = null!;
            if (value is null || !value.IsAbsoluteUri
                || (value.Scheme != Uri.UriSchemeHttps
                    && (value.Scheme != Uri.UriSchemeHttp || !value.IsLoopback))
                || value.AbsolutePath != "/" || value.UserInfo.Length != 0
                || value.Query.Length != 0 || value.Fragment.Length != 0)
                return false;
            normalized = new Uri(value.GetLeftPart(UriPartial.Authority) + "/", UriKind.Absolute);
            return true;
        }

        public static bool SameOrigin(Uri first, Uri second)
        {
            return first.IsAbsoluteUri && second.IsAbsoluteUri
                && string.Equals(first.Scheme, second.Scheme, StringComparison.OrdinalIgnoreCase)
                && string.Equals(first.IdnHost, second.IdnHost, StringComparison.OrdinalIgnoreCase)
                && first.Port == second.Port;
        }
    }
}
