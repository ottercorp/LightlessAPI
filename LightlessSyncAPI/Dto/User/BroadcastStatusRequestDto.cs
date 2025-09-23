using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class BroadcastStatusRequestDto
{
    public required string HashedCID { get; init; }
    public required bool Enabled { get; init; }
    public string? GID { get; init; }
}

