using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public record ProfileHousingDto(
    int? District = null,
    int? Ward = null,
    int? Plot = null,
    bool? IsLocked = null);
