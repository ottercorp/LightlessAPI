using MessagePack;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupData(string GID, string? Alias = null, DateTime? CreatedAt = null)
{
    [IgnoreMember]
    public string AliasOrGID => string.IsNullOrWhiteSpace(Alias) ? GID : Alias;
    [IgnoreMember]
    public DateTime? CreatedAt { get; set; } = CreatedAt;

}