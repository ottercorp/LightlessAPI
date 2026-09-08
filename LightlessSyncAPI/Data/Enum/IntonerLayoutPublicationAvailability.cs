namespace LightlessSync.API.Data.Enum;

/// <summary>
/// Whether a published Intoner layout is available.
/// A missing publication has a null summary, not a value in this enum.
/// </summary>
public enum IntonerLayoutPublicationAvailability : byte
{
    /// <summary>Availability is unknown. Do not load this layout.</summary>
    Unknown = 0,

    /// <summary>The publication may be subscribed to.</summary>
    Available = 1,

    /// <summary>The layout must not be loaded for subscribers. Owners and moderators may still preview it.</summary>
    Disabled = 2,
}
