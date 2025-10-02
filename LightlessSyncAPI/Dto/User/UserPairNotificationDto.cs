using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class UserPairNotificationDto
{
    public required string myHashedCid { get; init; }
    public required string message { get; init; }
}