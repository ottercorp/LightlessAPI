using MessagePack;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

[MessagePackObject]
public sealed record IntonerLayoutLocationDto(
    [property: Key(0)] IntonerLayoutWorldSelectionDto Worlds,
    [property: Key(1)] uint TerritoryId,
    [property: Key(2)] string TerritoryName,
    [property: Key(3)] uint DivisionId,
    [property: Key(4)] uint WardId,
    [property: Key(5)] uint HouseId,
    [property: Key(6)] uint RoomId);

[MessagePackObject]
public sealed record IntonerLayoutWorldSelectionDto(
    [property: Key(0)] bool AllDataCenters,
    [property: Key(1)] uint[] DataCenterIds,
    [property: Key(2)] ushort[] WorldIds);
