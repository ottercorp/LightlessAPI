using LightlessSync.API.Data;
using LightlessSync.API.Dto.CharaData;
using MessagePack;

namespace LightlessSync.API.Dto.User;


[MessagePackObject(keyAsPropertyName: true)]
public record LocationDto(UserData user, LocationInfo location);