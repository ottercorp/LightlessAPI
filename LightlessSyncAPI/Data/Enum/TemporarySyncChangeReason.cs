namespace LightlessSync.API.Data.Enum;

public enum TemporarySyncChangeReason : byte
{
    Snapshot,
    ClaimsActivated,
    ClaimExpired,
    PeerNoLongerObserved,
    ReciprocalClaimUnavailable,
    SourceCleared,
    SessionEnded,
    SessionReplaced,
    SourceDisabled,
    PairBlocked,
    PermissionsChanged,
}
