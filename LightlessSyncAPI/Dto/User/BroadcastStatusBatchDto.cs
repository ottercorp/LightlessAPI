using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class BroadcastStatusBatchDto
{
    public Dictionary<string, BroadcastStatusInfoDto> Results { get; init; } = new();
}
