using LightlessSync.API.Data;
using LightlessSync.API.Dto.CharaData;
using MessagePack;

namespace LightlessSync.API.Dto.User;


[MessagePackObject(keyAsPropertyName: true)]
public record LocationDto(UserData User, LocationInfo Location);

[MessagePackObject(keyAsPropertyName: true)]
public record LocationWithTimeDto(LocationDto LocationDto, DateTimeOffset ExpireAt);

[MessagePackObject(keyAsPropertyName: true)]
public record SharingStatusDto(UserData User, DateTimeOffset ExpireAt);
