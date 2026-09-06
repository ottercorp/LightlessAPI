namespace LightlessSync.API.Data.Enum;

public enum IntonerLayoutPublicationMutationStatus
{
    Unknown = 0,
    Success = 1,
    Conflict = 2,
    FeatureDisabled = 3,
    NotAuthorized = 4,
    InvalidTarget = 5,
    InvalidSnapshot = 6,
    UnsupportedSnapshotVersion = 7,
    QuotaExceeded = 8,
    MissingResource = 9,
}
