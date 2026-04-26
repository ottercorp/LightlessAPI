using LightlessSync.API.Data;
using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record PairModDeltaDto
{
    public Dictionary<ObjectKind, List<FileReplacementData>> FileReplacements { get; init; } = new();

    [IgnoreMember]
    public PairChangeType Types
        => FileReplacements.Count > 0 ? PairChangeType.ModdedPaths : PairChangeType.None;
}
