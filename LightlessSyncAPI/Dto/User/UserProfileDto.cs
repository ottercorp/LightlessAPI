using LightlessSync.API.Data;
using LightlessSync.API.Dto.Profile;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record UserProfileDto(
    UserData User,
    bool Disabled,
    bool? IsNSFW,
    string? ProfilePictureBase64,
    string? BannerPictureBase64,
    string? Description,
    int[]? Tags,
    ProfileColorsDto? Colors = null,
    ProfileHousingDto? Housing = null,
    ProfileWorldDto? World = null,
    ProfileTimeDto? Time = null) : UserDto(User);
