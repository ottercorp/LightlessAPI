using MessagePack;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

[MessagePackObject]
public sealed record IntonerLayoutLocationDto(
    [property: Key(0)] ushort WorldId,
    [property: Key(1)] string WorldName,
    [property: Key(2)] uint TerritoryId,
    [property: Key(3)] string TerritoryName,
    [property: Key(4)] uint DivisionId,
    [property: Key(5)] uint WardId,
    [property: Key(6)] uint HouseId,
    [property: Key(7)] uint RoomId);
