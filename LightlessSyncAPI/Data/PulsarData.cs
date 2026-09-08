using MessagePack;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class PulsarData
{
    public string CurrentHash { get; set; } = string.Empty;
    public string PrefetchHash { get; set; } = string.Empty;
    public string Cursor { get; set; } = string.Empty;  // opaque, relayed verbatim
}
