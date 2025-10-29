using MessagePack;

namespace LightlessSync.API.Dto.Group;

[MessagePackObject(keyAsPropertyName: true)]
public record GroupProfileAvailabilityRequest(string GID);
