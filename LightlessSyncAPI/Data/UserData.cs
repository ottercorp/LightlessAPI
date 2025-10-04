using MessagePack;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public record UserData(
    string UID,
    string? Alias = null,
    bool IsAdmin = false,
    bool IsModerator = false,
    bool HasVanity = false,
    string? TextColorHex = "",
    string? TextGlowColorHex = "")
{
    [IgnoreMember]
    public string AliasOrUID => string.IsNullOrWhiteSpace(Alias) ? UID : Alias;
}