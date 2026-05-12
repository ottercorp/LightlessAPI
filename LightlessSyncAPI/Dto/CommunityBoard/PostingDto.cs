using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.CommunityBoard;

[MessagePackObject(keyAsPropertyName: true)]
public record PostingLocationDto
{
    public int Type { get; init; }
    public uint ServerId { get; init; }
    public uint TerritoryId { get; init; }
    public uint MapId { get; init; }
    public uint Division { get; init; }
    public uint InstanceId { get; init; }
    public uint PlotId { get; init; }
    public uint HouseId { get; init; }
    public uint RoomId { get; init; }
    public bool IsSubDivision { get; init; }
    public bool IsHouseLocked { get; init; }
    public uint AetheryteId { get; init; }
    public string? CustomString { get; init; }
}

[MessagePackObject(keyAsPropertyName: true)]
public record PostingDto
{
    public Guid Guid { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset EndTime { get; init; }
    public DateTimeOffset LastUpdate { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int[] Tags { get; init; } = [];
    public World ServerId { get; init; }
    public bool IsNSFW { get; init; }
    public bool Open { get; init; }
    public bool HasTempGroup { get; init; }
    public string? TempGroupPW { get; init; }
    public string? UserUID { get; init; }
    public string? GroupGID { get; init; }
    public PostingLocationDto? Location { get; init; }
    public string? ImageUrl { get; init; }
    public string? ImageETag { get; init; }
}

[MessagePackObject(keyAsPropertyName: true)]
public record PostingCreateRequest
{
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset EndTime { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int[] Tags { get; init; } = [];
    public World ServerId { get; init; }
    public bool IsNSFW { get; init; }
    public bool Open { get; init; }
    public bool HasTempGroup { get; init; }
    public string? TempGroupPW { get; init; }
    public string? GroupGID { get; init; }
    public string? ImageBase64 { get; init; }
    public PostingLocationDto? Location { get; init; }
}
