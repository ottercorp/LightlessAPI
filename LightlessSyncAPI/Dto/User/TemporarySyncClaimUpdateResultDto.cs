using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record TemporarySyncClaimUpdateResultDto
{
    public TemporarySyncClaimUpdateStatus Status { get; set; }
    public int RetryAfterMilliseconds { get; set; }
    public List<TemporarySyncPairDto> Pairs { get; set; } = [];
}
