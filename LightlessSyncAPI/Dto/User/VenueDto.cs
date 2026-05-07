using LightlessSync.API.Data;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record VenueDto(List<VenueData> list, VenueData? lastVenue);