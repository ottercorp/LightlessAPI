using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record ProfilePresentationDto(
    ProfileLayoutMode? Layout = null,
    ProfileOpenMode? OpenMode = null,
    ProfileSide? ProfileSide = null,
    ProfileSide? QuickProfileSide = null,
    int? QuickProfileContentOffset = null)
{
    public const int MinimumQuickProfileContentOffset = -200;
    public const int MaximumQuickProfileContentOffset = 150;
}
