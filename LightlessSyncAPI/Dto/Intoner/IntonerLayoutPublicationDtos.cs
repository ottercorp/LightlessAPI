using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutPublicationTargetDto(
    IntonerLayoutPublicationTargetKind Kind,
    string? GroupGID = null);

/// <summary>
/// SnapshotBytes contains the normalized snapshot serialized as uncompressed MessagePack.
/// Serialize this request as MessagePack, then compress it with Brotli for upload.
/// </summary>
[MessagePackObject]
public sealed record IntonerLayoutPublishRequestDto(
    [property: Key(0)] Guid OperationId,
    [property: Key(1)] long ExpectedPublicationRevision,
    [property: Key(2)] string Name,
    [property: Key(3)] string? Description,
    [property: Key(4)] byte[] SnapshotBytes);

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutPublicationMutationResultDto(
    IntonerLayoutPublicationMutationStatus Status,
    IntonerLayoutSummaryDto? CurrentPublication,
    string Message = "",
    long CurrentPublicationRevision = 0);
