using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class BroadcastStatusInfoDto
{
    public required string HashedCID { get; init; }
    public required bool IsBroadcasting { get; init; }
    public TimeSpan? TTL { get; init; }
    public string? GID { get; init; }
}