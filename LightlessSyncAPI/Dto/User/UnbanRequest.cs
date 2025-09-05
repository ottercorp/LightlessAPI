using MessagePack;

namespace LightlessSync.API.Dto.User;
[MessagePackObject(keyAsPropertyName: true)]
public record UnbanRequest(string? Uid, string? DiscordId)
{
    public bool IsValid => !string.IsNullOrEmpty(Uid) || !string.IsNullOrEmpty(DiscordId);
}

