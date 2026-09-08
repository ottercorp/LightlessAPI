namespace LightlessSync.API.Data.Enum;

public enum IntonerLayoutIntentMutationStatus : byte
{
    Unknown = 0,
    Success = 1,
    RejectedFeatureUnavailable = 2,
    RejectedNotAuthorized = 3,
    RejectedPublicationUnavailable = 4,
}
