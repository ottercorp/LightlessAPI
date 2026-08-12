using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record ProfilePortraitFrameDto(byte[] Pixels, uint[] Palette)
{
    public const int GridWidth = 64;
    public const int GridHeight = 96;
    public const int BorderDepth = 8;
    public const int PaletteColorCount = 16;
    public const int PixelCount = GridWidth * GridHeight;
}
