using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record UserModDataMessageDto(
    List<UserData> Recipients,
    PairModDeltaDto ModData,
    bool HasManipulationUpdate = false,
    string ManipulationData = "",
    CensusDataDto? CensusDataDto = null);
