using MessagePack;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupData(string GID, string? Alias = null, DateTime? CreatedAt = null, string? Description = null, string? Tags = null, string? Avatar = null)
{
    [IgnoreMember]
    public string AliasOrGID => string.IsNullOrWhiteSpace(Alias) ? GID : Alias;
    [IgnoreMember]
    public DateTime? CreatedAt { get; set; } = CreatedAt;
    [IgnoreMember]
    public string? Description { get; set; } = Description;
    [IgnoreMember]
    public string? Tags { get; set; } = Tags;
    [IgnoreMember]
    public string? Avatar { get; set; } = Avatar;

}