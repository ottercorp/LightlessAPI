using LightlessSync.API.Data;

namespace LightlessSync.API.Dto.Group
{
    public record GroupPruneSettingsDto(GroupData Group, bool AutoPruneEnabled, int AutoPruneDays);
}
