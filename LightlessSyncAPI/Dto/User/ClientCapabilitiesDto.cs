using System.Text.Json;

namespace LightlessSync.API.Dto.User;

// All fields MUST have a default. This is sent by the client in the header.
// Old clients will NOT send all fields. Third-party clients will NOT send all fields.
// Use a reasonable default for each.
public record ClientCapabilitiesDto(
    string ClientVersion = "unknown",

    // Whether or not "delta updates" are supported. Specifically, whether the client
    // supports the PairReceiveVisualSingle, PairReceiveVisualDelta, and PairReceiveModDelta methods.
    bool DeltaUpdates = false,

    // Whether temporary sync claims and edge updates are supported.
    bool TemporarySync = false
)
{
    public const string ClientCapabilitiesHeader = "X-Client-Capabilities";

    public string ToHeaderValue()
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(this);
        return Convert.ToBase64String(json);
    }

    public static ClientCapabilitiesDto? FromHeaderValue(string? val)
    {
        if (string.IsNullOrEmpty(val)) return null;

        try
        {
            var bytes = Convert.FromBase64String(val);
            return JsonSerializer.Deserialize<ClientCapabilitiesDto>(bytes);
        }
        catch { return null; }
    }
}
