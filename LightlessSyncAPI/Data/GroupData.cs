using MessagePack;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupData(string GID, string? Alias = null, DateTime? CreatedAt = null, bool HasPassword = false)
{
    [IgnoreMember]
    public string AliasOrGID => Alias ?? GID;
}