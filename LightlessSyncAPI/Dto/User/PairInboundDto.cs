using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record PairInboundDto<TPayload>(
    UserData User,
    TPayload Payload) : UserDto(User);
