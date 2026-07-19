using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record TemporarySyncClaimDto(
    TemporarySyncSource Source,
    long Revision,
    List<string> ObservedHashedCids);
