using LightlessSync.API.Data.Enum;
using LightlessSync.API.Dto.Intoner.Snapshot;
using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutSummaryDto(
    IntonerLayoutRefDto LayoutRef,
    string Name,
    string? Description,
    int ObjectCount,
    int ResourceCount,
    long TotalResourceBytes,
    int LocationCount,
    DateTimeOffset UpdatedAtUtc,
    IntonerLayoutPublicationAvailability Availability,
    string? AvailabilityDisplayReason = null,
    IReadOnlyList<uint>? PreviewTerritoryIds = null,
    int TerritoryCount = 0,
    bool HasResidentialLocations = false,
    bool HasNonResidentialLocations = false)
{
    public const int MaxPreviewTerritories = 3;
    public const int MaxPreviewLocations = 3;
    public IReadOnlyList<IntonerLayoutLocationDto>? PreviewLocations { get; init; }
}
