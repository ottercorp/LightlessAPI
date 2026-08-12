using LightlessSync.API.Data;
using LightlessSync.API.Dto.Profile;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record UserProfileDto(
    UserData User,
    bool Disabled,
    bool? IsNSFW,
    string? Description,
    int[]? Tags,
    ProfileColorsDto? Colors = null,
    ProfileHousingDto? Housing = null,
    ProfileWorldDto? World = null,
    ProfileTimeDto? Time = null,
    string? ProfilePictureUrl = null,
    string? BannerPictureUrl = null,
    string? ProfilePictureETag = null,
    string? BannerPictureETag = null,
    string? PortraitPictureUrl = null,
    string? PortraitPictureETag = null,
    ProfileLayoutMode? Layout = null,
    ProfileOpenMode? OpenMode = null,
    string? PortraitFrameUrl = null,
    string? PortraitFrameETag = null) : UserDto(User);
