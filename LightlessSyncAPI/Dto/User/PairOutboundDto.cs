using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record PairOutboundDto<TPayload>(
    List<UserData> Recipients,
    TPayload Payload,
    CensusDataDto? CensusDataDto = null);
