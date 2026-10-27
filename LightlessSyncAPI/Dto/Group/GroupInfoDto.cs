using LightlessSync.API.Data;
using LightlessSync.API.Data.Enum;
using LightlessSync.API.Dto.Intoner;
using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupInfoDto(GroupData Group, UserData Owner, GroupPermissions GroupPermissions) : GroupDto(Group)
{
    public GroupPermissions GroupPermissions { get; set; } = GroupPermissions;
    public UserData Owner { get; set; } = Owner;

    [IgnoreMember]
    public string OwnerUID => Owner.UID;
    [IgnoreMember]
    public string? OwnerAlias => Owner.Alias;
    [IgnoreMember]
    public string OwnerAliasOrUID => Owner.AliasOrUID;
}

[MessagePackObject(keyAsPropertyName: true)]
public record GroupJoinInfoDto(GroupData Group, UserData Owner, GroupPermissions GroupPermissions, bool Success) : GroupInfoDto(Group, Owner, GroupPermissions)
{
    public IntonerLayoutSummaryDto? IntonerLayoutSummary { get; init; }
}
