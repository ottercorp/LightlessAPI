using LightlessSync.API.Data;

namespace LightlessSync.API.Dto.Group
{
    public record GroupProfileDto(GroupData Group, string? Description, string? Tags, string? PictureBase64) : GroupDto(Group);
}
