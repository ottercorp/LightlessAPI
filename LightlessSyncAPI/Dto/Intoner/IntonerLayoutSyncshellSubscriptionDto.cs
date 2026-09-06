using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutSyncshellSubscriptionDto(
    string GroupGID,
    bool Enabled);
