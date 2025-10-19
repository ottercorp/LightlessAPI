using LightlessSync.API.Data;

namespace LightlessSync.API.Dto.Group
{
    public record GroupProfileDto(GroupData Group, string? Description, int[]? Tags, string? PictureBase64, bool? IsNsfw, bool? IsDisabled) : GroupDto(Group);
}
