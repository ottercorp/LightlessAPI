using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public record ProfileTimeDto(
    int? UtcOffsetMinutes = null,
    bool? Use24Hour = null,
    bool? HasPlayWindow = null,
    int? PlayWindowStartMinutes = null,
    int? PlayWindowEndMinutes = null,
    bool? HasDayAvailability = null,
    byte? DaysMask = null);
