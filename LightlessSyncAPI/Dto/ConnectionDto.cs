using LightlessSync.API.Data;
using LightlessSync.API.Data.Enum;
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
    public TemporarySyncCapabilitiesDto TemporarySync { get; set; } = new();
    public ProfileMediaCapabilitiesDto ProfileMedia { get; set; } = new();
    public IntonerLayoutCapabilitiesDto IntonerLayouts { get; set; } = new();
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
    public bool SupportsXubc7DerivedDownload { get; set; }
    public string Xubc7DerivedProfile { get; set; } = string.Empty;
}

[MessagePackObject(keyAsPropertyName: true)]
public record TemporarySyncCapabilitiesDto
{
    public bool Supported { get; set; }
    public List<TemporarySyncSourceCapabilitiesDto> Sources { get; set; } = [];
}

[MessagePackObject(keyAsPropertyName: true)]
public record TemporarySyncSourceCapabilitiesDto
{
    public TemporarySyncSource Source { get; set; }
    public int MaxObservedPeers { get; set; }
    public int ClaimTtlSeconds { get; set; }
}

[MessagePackObject(keyAsPropertyName: true)]
public record ProfileMediaCapabilitiesDto
{
    public const int CurrentContractVersion = 1;

    public bool Supported { get; set; }
    public int ContractVersion { get; set; }
    public bool Enabled { get; set; }
    public int MaxEncodedImageBytes { get; set; }
}

[MessagePackObject(keyAsPropertyName: true)]
public record IntonerLayoutCapabilitiesDto
{
    public bool Supported { get; set; }
    /// <summary>Whether Intoner layout sharing is currently enabled.</summary>
    public bool Enabled { get; set; }
    /// <summary>Snapshot format versions this server can validate and serve.</summary>
    public List<int> SupportedSnapshotFormatVersions { get; set; } = [];
    /// <summary>Snapshot limits are zero when no snapshot format version is supported.</summary>
    public int MaxCompressedSnapshotBytes { get; set; }
    public int MaxDecompressedSnapshotBytes { get; set; }
    public int MaxObjectCount { get; set; }
    public int MaxCollectionCount { get; set; }
    public int MaxRedirectCount { get; set; }
    public int MaxResourceCount { get; set; }
    public long MaxIndividualResourceBytes { get; set; }
    public long MaxTotalResourceBytes { get; set; }
    public int MaxNameCharacters { get; set; }
    public int MaxDescriptionCharacters { get; set; }
}
