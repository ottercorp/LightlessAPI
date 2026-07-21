using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record TemporarySyncPairDto(
    OnlineUserIdentDto User,
    TemporarySyncSource Sources,
    UserPermissions OwnPermissions,
    UserPermissions OtherPermissions,
    TemporarySyncChangeReason ChangeReason = TemporarySyncChangeReason.Snapshot);
