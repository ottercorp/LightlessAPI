using MessagePack;

namespace LightlessSync.API.Dto.Files;

[MessagePackObject(keyAsPropertyName: true)]
public record AscfResumeDto
{
    public string Hash { get; set; } = string.Empty;
    public string StorageHash { get; set; } = string.Empty;
    public string? Sha1Hash { get; set; }
    public string Blake3Hash { get; set; } = string.Empty;
    public long RawSize { get; set; }
    public long EncodedSize { get; set; }
    public int RawChunkSize { get; set; }
    public int ChunkCount { get; set; }
    public Guid StreamId { get; set; }
    public long RequestedEncodedBytes { get; set; }
    public int NextChunkIndex { get; set; }
    public long NextEncodedOffset { get; set; }
    public long NextRawOffset { get; set; }
    public bool IsComplete { get; set; }
    public bool CanResume { get; set; }
}
