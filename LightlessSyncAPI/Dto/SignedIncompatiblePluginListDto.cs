using MessagePack;

namespace LightlessSync.API.Dto;

[MessagePackObject(keyAsPropertyName: true)]
public record SignedIncompatiblePluginListDto
{
    public IReadOnlyList<IncompatiblePluginDto> Plugins { get; init; } = [];
    public long IssuedAtUnixSeconds { get; init; }
    public string Nonce { get; init; } = string.Empty;
    public string Signature { get; init; } = string.Empty;
}
