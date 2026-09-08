using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutCatalogDto(
    IntonerLayoutSummaryDto? OwnPersonalPublication,
    List<IntonerLayoutCatalogEntryDto> Entries,
    long OwnPersonalPublicationRevision = 0,
    IntonerLayoutPersonalAudienceDto? PersonalAudience = null)
{
    public const int MaxEntryRefreshBatchSize = 128;
}
