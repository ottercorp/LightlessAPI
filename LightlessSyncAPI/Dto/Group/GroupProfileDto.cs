using LightlessSync.API.Data;
using LightlessSync.API.Dto.Profile;

namespace LightlessSync.API.Dto.Group;

public record GroupProfileDto(
    GroupData Group,
    string? Description,
    int[]? Tags,
    bool? IsNsfw,
    bool? IsHidden,
    ProfileColorsDto? Colors = null,
    ProfileHousingDto? Housing = null,
    ProfileWorldDto? World = null,
    ProfileTimeDto? Time = null,
    string? PictureUrl = null,
    string? BannerUrl = null,
    string? PictureETag = null,
    string? BannerETag = null,
    int? MemberCount = null,
    bool IsModerationDisabled = false) : GroupDto(Group);
