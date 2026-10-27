using MessagePack;

namespace LightlessSync.API.Dto.User;
[MessagePackObject(keyAsPropertyName: true)]
public record UnbanRequest(string? Uid, string? DiscordId)
{
    [IgnoreMember]
    public bool IsValid => !string.IsNullOrEmpty(Uid) || !string.IsNullOrEmpty(DiscordId);
}

