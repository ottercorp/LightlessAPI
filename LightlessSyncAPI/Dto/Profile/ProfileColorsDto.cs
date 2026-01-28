using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public record ProfileColorsDto(
    string? BackgroundColorHex = null,
    string? AccentColorHex = null);
