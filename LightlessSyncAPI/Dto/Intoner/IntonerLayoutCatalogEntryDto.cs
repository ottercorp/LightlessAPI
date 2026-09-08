using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutCatalogEntryDto(
    IntonerLayoutActivationKeyDto ActivationKey,
    IntonerLayoutSummaryDto? Summary,
    bool ReceiveIntent = false,
    long PublicationRevision = 0);
