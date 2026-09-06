using MessagePack;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

/// <summary>
/// Published layout content with file hashes instead of paths on the sender's computer.
/// </summary>
[MessagePackObject]
public sealed record IntonerLayoutSnapshotDto(
    [property: Key(0)] int FormatVersion,
    [property: Key(1)] int IntonerApiBreakingVersion,
    [property: Key(2)] int IntonerApiFeatureVersion,
    [property: Key(3)] int ObjectCount,
    [property: Key(4)] byte[] ObjectData,
    [property: Key(5)] List<IntonerLayoutLocationDto> Locations,
    [property: Key(6)] List<IntonerLayoutCollectionDto> Collections,
    [property: Key(7)] List<IntonerLayoutResourceDto> Resources)
{
    public const int CurrentFormatVersion = 1;
}
