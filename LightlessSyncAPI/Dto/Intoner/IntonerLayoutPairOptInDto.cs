using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutPairOptInDto(
    string PublisherUID,
    bool Enabled);
