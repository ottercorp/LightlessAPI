using LightlessSync.API.Dto.CharaData;
using MessagePack;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public record VenueData(
    Guid Id,
    string Title,
    string? Description,
    UserData user,
    DateTimeOffset UpdatedAt,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    LocationInfo? Location,
    string? GroupGID,
    string? Password,
    int[] Tags);