using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record LocationSharingToggleDto(List<string> users, DateTimeOffset duration);