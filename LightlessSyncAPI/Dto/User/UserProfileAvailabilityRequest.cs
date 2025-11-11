using MessagePack;

namespace LightlessSync.API.Dto.User;
[MessagePackObject(keyAsPropertyName: true)]
public record UserProfileAvailabilityRequest(string UID);
