using LightlessSync.API.Data;
using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupJoinHashedDto(
    GroupData Group,
    string HashedPassword,
    GroupUserPreferredPermissions GroupUserPreferredPermissions
);
