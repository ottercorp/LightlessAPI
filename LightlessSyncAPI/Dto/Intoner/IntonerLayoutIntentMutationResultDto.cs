using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

/// <param name="Entry">The updated catalog entry after the subscribe or unsubscribe request, even if it failed.</param>
[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutIntentMutationResultDto(
    IntonerLayoutIntentMutationStatus Status,
    IntonerLayoutCatalogEntryDto Entry);
