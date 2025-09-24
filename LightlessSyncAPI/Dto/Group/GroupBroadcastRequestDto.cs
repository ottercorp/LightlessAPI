using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class GroupBroadcastRequestDto
{
    public required string GID { get; init; }
    public string? HashedCID { get; set; }
    public required bool Enabled { get; init; }
}
