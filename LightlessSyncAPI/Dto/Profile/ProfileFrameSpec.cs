namespace LightlessSync.API.Dto.Profile;

public readonly record struct ProfileFrameSpec(
    int GridWidth,
    int GridHeight,
    int BorderDepth,
    int OutsetCells,
    int RenderCellSize,
    int LogicalCellSize)
{
    public const int PortraitGridWidth = 128;
    public const int PortraitGridHeight = 192;
    public const int PortraitBorderDepth = 16;
    public const int QuickProfileGridWidth = 98;
    public const int QuickProfileGridHeight = 148;
    public const int QuickProfileBorderDepth = 6;
    public const int QuickProfileOutsetCells = 4;

    public int GridCellCount => GridWidth * GridHeight;
    public int BorderPixelCount
        => GridCellCount - (GridWidth - BorderDepth * 2) * (GridHeight - BorderDepth * 2);
    public int OutputWidth => GridWidth * RenderCellSize;
    public int OutputHeight => GridHeight * RenderCellSize;
    public int LogicalOutset => OutsetCells * LogicalCellSize;
    public int LogicalInset => (BorderDepth - OutsetCells) * LogicalCellSize;

    public bool TryGetBorderPixelIndex(int x, int y, out int index)
    {
        index = -1;
        if ((uint)x >= GridWidth || (uint)y >= GridHeight)
            return false;

        int middleHeight = GridHeight - BorderDepth * 2;
        if (y < BorderDepth)
        {
            index = y * GridWidth + x;
            return true;
        }
        if (y >= GridHeight - BorderDepth)
        {
            index = BorderDepth * GridWidth
                + middleHeight * BorderDepth * 2
                + (y - (GridHeight - BorderDepth)) * GridWidth
                + x;
            return true;
        }

        int middleRowStart = BorderDepth * GridWidth + (y - BorderDepth) * BorderDepth * 2;
        if (x < BorderDepth)
        {
            index = middleRowStart + x;
            return true;
        }
        if (x >= GridWidth - BorderDepth)
        {
            index = middleRowStart + BorderDepth + x - (GridWidth - BorderDepth);
            return true;
        }

        return false;
    }

    public static ProfileFrameSpec Get(ProfileFrameKind kind)
        => kind switch
        {
            ProfileFrameKind.Portrait => new(
                PortraitGridWidth,
                PortraitGridHeight,
                PortraitBorderDepth,
                OutsetCells: 0,
                RenderCellSize: 4,
                LogicalCellSize: 4),
            ProfileFrameKind.QuickProfile => new(
                QuickProfileGridWidth,
                QuickProfileGridHeight,
                QuickProfileBorderDepth,
                QuickProfileOutsetCells,
                RenderCellSize: 8,
                LogicalCellSize: 4),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown profile frame kind")
        };
}
