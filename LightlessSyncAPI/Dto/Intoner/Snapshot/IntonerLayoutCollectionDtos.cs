using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

/// <summary>
/// A redirect to either a game file (GamePath) or a mod file (Blake3Hash),
/// without including the sender's local file path.
/// </summary>
[MessagePackObject]
public sealed record IntonerLayoutCollectionReplacementDto(
    [property: Key(0)] IntonerLayoutCollectionReplacementKind Kind,
    [property: Key(1)] string GamePath,
    [property: Key(2)] string Blake3Hash);

[MessagePackObject]
public sealed record IntonerLayoutCollectionRedirectDto(
    [property: Key(0)] string RequestedGamePath,
    [property: Key(1)] IntonerLayoutCollectionReplacementDto Replacement);

[MessagePackObject]
public sealed record IntonerLayoutCollectionDto(
    [property: Key(0)] string CollectionId,
    [property: Key(1)] string Name,
    [property: Key(2)] List<IntonerLayoutCollectionRedirectDto> Redirects);

[MessagePackObject]
public sealed record IntonerLayoutResourceDto(
    [property: Key(0)] string Blake3Hash,
    [property: Key(1)] long RawBytes);
