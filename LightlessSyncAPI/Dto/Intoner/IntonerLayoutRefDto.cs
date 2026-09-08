using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

/// <param name="SnapshotSha256">
/// Lowercase hexadecimal SHA-256 of the normalized, uncompressed snapshot bytes.
/// Hash only the snapshot, without HTTP headers or the surrounding publish request.
/// </param>
[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutRefDto(
    Guid LayoutId,
    long PublicationRevision,
    int SnapshotFormatVersion,
    string SnapshotSha256)
{
    public const int Sha256HexLength = 64;
}
