using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupDto(GroupData Group)
{
    public GroupData Group { get; set; } = Group;
    [IgnoreMember]
    public string GID => Group.GID;
    [IgnoreMember]
    public string? GroupAlias => Group.Alias;
    [IgnoreMember]
    public string GroupAliasOrGID => Group.AliasOrGID;
}