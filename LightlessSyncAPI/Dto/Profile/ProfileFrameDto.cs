using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record ProfileFrameDto(byte[] BorderPixels, uint[] Palette)
{
    public const int PaletteColorCount = 32;
}
