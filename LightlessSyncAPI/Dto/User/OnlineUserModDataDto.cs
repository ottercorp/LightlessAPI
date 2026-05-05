using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record OnlineUserModDataDto(
    UserData User,
    PairModDeltaDto ModData,
    bool HasManipulationUpdate = false,
    string ManipulationData = "") : UserDto(User);
