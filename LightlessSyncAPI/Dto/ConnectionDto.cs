using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto;

[MessagePackObject(keyAsPropertyName: true)]
public record ConnectionDto(UserData User)
{
    public Version CurrentClientVersion { get; set; } = new(0, 0, 0);
    public int ServerVersion { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsModerator { get; set; }
    public bool HasVanity { get; set; }
    public string? TextColorHex { get; set; }
    public string? TextGlowColorHex { get; set; }
    public ServerInfo ServerInfo { get; set; } = new();
    public ServerCapabilitiesDto ServerCapabilities { get; set; } = new();
    public DefaultPermissionsDto DefaultPreferredPermissions { get; set; } = new();
}

[MessagePackObject(keyAsPropertyName: true)]
public record ServerInfo
{
    public string ShardName { get; set; } = string.Empty;
    public int MaxGroupUserCount { get; set; }
    public int MaxGroupsCreatedByUser { get; set; }
    public int MaxGroupsJoinedByUser { get; set; }
    public Uri FileServerAddress { get; set; } = new Uri("http://nonemptyuri");
    public int MaxCharaData { get; set; }
    public int MaxCharaDataVanity { get; set; }
}

[MessagePackObject(keyAsPropertyName: true)]
public record ServerCapabilitiesDto
{
    public FileTransferCapabilitiesDto FileTransfers { get; set; } = new();
}

[MessagePackObject(keyAsPropertyName: true)]
public record FileTransferCapabilitiesDto
{
    public bool SupportsAscfUpload { get; set; }
    public bool SupportsAscfDownload { get; set; }
    public bool SupportsAscfResume { get; set; }
    public bool SupportsAscfChunkUpload { get; set; }
    public int AscfUploadChunkSizeBytes { get; set; }
    public long MaxUploadSizeBytes { get; set; }
    public bool SupportsXuastcDerivedDownload { get; set; }
    public string XuastcDerivedProfile { get; set; } = string.Empty;
}