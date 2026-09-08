using MessagePack;

namespace LightlessSync.API.Dto.Profile;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record ProfileFrameAssetDto(ProfileFrameKind Kind, string Url, string ETag);
