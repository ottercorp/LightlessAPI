using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.Intoner;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record IntonerLayoutActivationKeyDto(
    IntonerLayoutActivationKind Kind,
    string Id);
