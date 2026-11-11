using LightlessSync.API.Routes;
using MessagePack;
using MessagePack.Resolvers;

namespace LightlessSync.API.Dto.Files;

/// <summary> A metadata request </summary>
/// <param name="Hashes"> Requested content hashes </param>
/// <param name="PreferredRegionId"> Optional public region ID </param>
public sealed record DownloadPlanRequestDto(List<string> Hashes, string? PreferredRegionId = null);

/// <summary> A versioned batch of file facts and download endpoints. </summary>
[MessagePackObject]
public sealed partial record DownloadPlanDto
{
    public const int ContractVersion = 1;
    public const string MediaType = "application/vnd.lightless.download-plan.v1+msgpack";
    public const int MaximumBytes = 64 * 1024 * 1024;
    private static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard
        .WithResolver(StandardResolver.Instance)
        .WithCompression(MessagePackCompression.Lz4BlockArray)
        .WithSecurity(MessagePackSecurity.UntrustedData.WithMaximumObjectGraphDepth(128)
            .WithMaximumDecompressedSize(MaximumBytes));

    [Key(0)] public string DetectedContinent { get; set; } = string.Empty;
    [Key(1)] public string Format { get; set; } = string.Empty;
    [Key(2)] public DownloadEndpointDto[] Endpoints { get; set; } = [];
    [Key(3)] public DownloadPlanFileDto[] Files { get; set; } = [];

    /// <summary> Serializes a validated plan using the version's fixed wire encoding. </summary>
    public byte[] Serialize()
    {
        Validate();
        byte[] bytes = MessagePackSerializer.Serialize(this, Options);
        if (bytes.Length > MaximumBytes)
            throw new InvalidDataException("Download plan exceeds the response limit.");
        MessagePackReader reader = new(bytes);
        _ = reader.ReadArrayHeader();
        if (reader.NextMessagePackType == MessagePackType.Extension)
        {
            var header = reader.ReadExtensionFormat();
            if (header.TypeCode != ReservedExtensionTypeCodes.Lz4BlockArray)
                throw new InvalidDataException("Download plan uses an unexpected compression envelope.");

            MessagePackReader lengths = new(header.Data);
            int remaining = MaximumBytes;
            while (!lengths.End)
            {
                int length = lengths.ReadInt32();
                if ((uint)length > (uint)remaining)
                    throw new InvalidDataException("Download plan exceeds the decompressed response limit.");
                remaining -= length;
            }
        }
        return bytes;
    }

    /// <summary> Reads and validates an untrusted response before any endpoint is used. </summary>
    public static DownloadPlanDto Deserialize(ReadOnlyMemory<byte> bytes)
    {
        if (bytes.Length > MaximumBytes)
            throw new InvalidDataException("Download plan exceeds the response limit.");
        DownloadPlanDto? plan = MessagePackSerializer.Deserialize<DownloadPlanDto>(bytes, Options);
        if (plan is null) throw new InvalidDataException("Download plan cannot be null.");
        plan.Validate();
        return plan;
    }

    /// <summary> Resolves a permitted candidate using the existing file URL contract. </summary>
    public Uri Resolve(DownloadPlanFileDto file, DownloadPlanCandidateDto candidate)
        => LightlessFiles.ServerFilesDirectDownloadFullPath(Endpoints[candidate.EndpointIndex].BaseUri,
            file.RouteHashes[candidate.HashIndex], Format == LightlessFiles.FileFormatAscf ? Format : null);

    /// <summary> Expands the validated wire representation for existing transfer processing. </summary>
    public List<DownloadFileDto> CreateTransfers()
    {
        var templates = new (string Prefix, string Suffix)[Endpoints.Length];
        for (int index = 0; index < Endpoints.Length; ++index)
        {
            Uri endpoint = Endpoints[index].BaseUri;
            templates[index] = (LightlessFiles.ServerFilesDirectDownloadFullPath(endpoint, string.Empty).ToString(),
                Format == LightlessFiles.FileFormatAscf
                    ? LightlessFiles.ServerFilesDirectDownloadFullPath(endpoint, string.Empty, Format).Query
                    : string.Empty);
        }
        List<DownloadFileDto> result = new(Files.Length);
        foreach (DownloadPlanFileDto file in Files)
        {
            result.Add(new DownloadFileDto
            {
                Hash = file.Hash, Size = file.Size, RawSize = file.RawSize, FileExists = file.FileExists,
                IsForbidden = file.IsForbidden, ForbiddenBy = file.ForbiddenBy,
                QueueRouteHashes = file.FileExists && !file.IsForbidden ? [.. file.RouteHashes] : [],
                Url = file.QueueEndpoint is { } index ? Endpoints[index].BaseUri.ToString() : string.Empty,
                CDNDownloadUrl = file.Cdn is { } cdn ? ResolveUrl(file, cdn, templates) : string.Empty,
                HasFileDirectUrls = ResolveCandidates(file, file.Available, templates),
                PullThroughDirectUrls = ResolveCandidates(file, file.PullThrough, templates),
                DerivedFiles = file.Derived is null ? null : [.. file.Derived],
            });
        }
        return result;
    }

    private static string ResolveUrl(DownloadPlanFileDto file, DownloadPlanCandidateDto candidate,
        (string Prefix, string Suffix)[] templates)
    {
        var template = templates[candidate.EndpointIndex];
        return string.Concat(template.Prefix, file.RouteHashes[candidate.HashIndex], template.Suffix);
    }

    private static List<string> ResolveCandidates(DownloadPlanFileDto file, DownloadPlanCandidateDto[] candidates,
        (string Prefix, string Suffix)[] templates)
    {
        List<string> urls = new(candidates.Length);
        foreach (DownloadPlanCandidateDto candidate in candidates)
            urls.Add(ResolveUrl(file, candidate, templates));
        return urls;
    }

    private void Validate()
    {
        if (Format is not (LightlessFiles.FileFormatAscf or LightlessFiles.FileFormatWrappedLz4)
            || DetectedContinent is null || Endpoints is null || Files is null)
            throw new InvalidDataException("Invalid download plan context.");
        foreach (DownloadEndpointDto endpoint in Endpoints)
        {
            if (endpoint is null || string.IsNullOrWhiteSpace(endpoint.ShardId)
                || (endpoint.RegionId is null) != (endpoint.RegionName is null)
                || endpoint.RegionId is not null && (string.IsNullOrWhiteSpace(endpoint.RegionId) || string.IsNullOrWhiteSpace(endpoint.RegionName))
                || endpoint.Continents is null
                || endpoint.Continents.Any(string.IsNullOrWhiteSpace) || !LightlessFiles.IsDownloadUri(endpoint.BaseUri))
                throw new InvalidDataException("Invalid download endpoint.");
        }
        HashSet<string> hashes = new(Files.Length, StringComparer.OrdinalIgnoreCase);
        foreach (DownloadPlanFileDto file in Files)
        {
            if (file is null || file.RouteHashes is null || (uint)file.HashIndex >= (uint)file.RouteHashes.Length
                || file.RouteHashes.Any(hash => !IsHash(hash)) || !hashes.Add(file.Hash)
                || file.Size < 0 || file.RawSize < 0 || file.ForbiddenBy is null
                || file.Available is null || file.PullThrough is null
                || file.QueueEndpoint is { } index && (uint)index >= (uint)Endpoints.Length)
                throw new InvalidDataException("Invalid download file.");
            CheckCandidates(file, file.Available);
            CheckCandidates(file, file.PullThrough);
            if (file.Cdn is { } cdn) CheckCandidate(file, cdn);
            if ((!file.FileExists || file.IsForbidden) && (file.QueueEndpoint is not null
                || file.Cdn is not null || file.Available.Length != 0 || file.PullThrough.Length != 0
                || file.Derived is { Length: > 0 }))
                throw new InvalidDataException("Unavailable file advertises download routes.");
            foreach (DownloadFileDerivedDto derived in file.Derived ?? [])
            {
                if (derived is null || derived.Kind is null || derived.Profile is null || derived.Size < 0
                    || derived.DirectUrls is null || derived.DirectUrls.Any(url => !Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) || !LightlessFiles.IsDownloadUri(uri)))
                    throw new InvalidDataException("Invalid derived download.");
            }
        }
    }

    private void CheckCandidates(DownloadPlanFileDto file, ReadOnlySpan<DownloadPlanCandidateDto> candidates)
    {
        foreach (DownloadPlanCandidateDto candidate in candidates) CheckCandidate(file, candidate);
    }

    private void CheckCandidate(DownloadPlanFileDto file, DownloadPlanCandidateDto candidate)
    {
        if ((uint)candidate.EndpointIndex >= (uint)Endpoints.Length
            || (uint)candidate.HashIndex >= (uint)file.RouteHashes.Length)
            throw new InvalidDataException("Download candidate references an absent endpoint or hash.");
    }

    private static bool IsHash(string? hash)
    {
        if (hash is not { Length: 40 or 64 }) return false;
        foreach (char value in hash)
            if (!char.IsAsciiHexDigit(value)) return false;
        return true;
    }
}

/// <summary> A public download address with an opaque shard identity and optional region metadata. </summary>
/// <param name="ShardId"> Identity shared by addresses belonging to one shard. </param>
/// <param name="RegionId"> Stable preference ID, or null when only automatic selection is published. </param>
/// <param name="BaseUri"> Address the client can contact. </param>
/// <param name="Continents"> Location hints for initial source selection. </param>
/// <param name="RegionName"> Display label paired with the region ID, null when the ID is absent. </param>
[MessagePackObject]
public sealed partial record DownloadEndpointDto(
    [property: Key(0)] string ShardId,
    [property: Key(1)] string? RegionId,
    [property: Key(2)] Uri BaseUri,
    [property: Key(3)] string[] Continents,
    [property: Key(4)] string? RegionName = null);

/// <summary> References a batch endpoint and an alias in the containing file's hash table. </summary>
[MessagePackObject]
public readonly partial record struct DownloadPlanCandidateDto(
    [property: Key(0)] int EndpointIndex, [property: Key(1)] int HashIndex);

/// <summary> File facts and eligible routes </summary>
[MessagePackObject]
public sealed partial record DownloadPlanFileDto
{
    [Key(0)] public int HashIndex { get; set; }
    [IgnoreMember] public string Hash => RouteHashes[HashIndex];
    [Key(1)] public long Size { get; set; }
    [Key(2)] public long RawSize { get; set; }
    [Key(3)] public bool FileExists { get; set; }
    [Key(4)] public bool IsForbidden { get; set; }
    [Key(5)] public string ForbiddenBy { get; set; } = string.Empty;
    [Key(6)] public string[] RouteHashes { get; set; } = [];
    [Key(7)] public int? QueueEndpoint { get; set; }
    [Key(8)] public DownloadPlanCandidateDto? Cdn { get; set; }
    [Key(9)] public DownloadPlanCandidateDto[] Available { get; set; } = [];
    [Key(10)] public DownloadPlanCandidateDto[] PullThrough { get; set; } = [];
    [Key(11)] public DownloadFileDerivedDto[]? Derived { get; set; }
}
