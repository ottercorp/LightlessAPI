using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record UserVanityColorsDto(string? TextColorHex, string? TextGlowColorHex);
