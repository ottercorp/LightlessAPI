using LightlessSync.API.Data;
using LightlessSync.API.Dto.Profile;

namespace LightlessSync.API.Dto.Group;

public record GroupProfileDto(
    GroupData Group,
    string? Description,
    int[]? Tags,
    string? PictureBase64,
    string? BannerBase64,
    bool? IsNsfw,
    bool? IsDisabled,
    ProfileColorsDto? Colors = null,
    ProfileHousingDto? Housing = null,
    ProfileWorldDto? World = null,
    ProfileTimeDto? Time = null) : GroupDto(Group);
