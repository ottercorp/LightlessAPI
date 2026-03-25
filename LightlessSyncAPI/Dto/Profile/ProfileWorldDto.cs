using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public record ProfileWorldDto(
    uint? DataCenterId = null,
    ushort? WorldId = null);
