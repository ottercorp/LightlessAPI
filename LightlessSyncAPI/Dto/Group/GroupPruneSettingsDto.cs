using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupPruneSettingsDto(GroupData Group, bool AutoPruneEnabled, int AutoPruneDays);