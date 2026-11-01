using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupPairDto(GroupData Group, UserData User) : GroupDto(Group)
{
    [IgnoreMember]
    public string UID => User.UID;
    [IgnoreMember]
    public string? UserAlias => User.Alias;
    [IgnoreMember]
    public string UserAliasOrUID => User.AliasOrUID;

}