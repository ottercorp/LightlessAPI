using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record PairVisualDeltaEntryDto
{
    public PairVisualDeltaEntryDto()
    {
    }

    public PairVisualDeltaEntryDto(ObjectKind targetObject, PairChangeType type, string data)
    {
        TargetObject = targetObject;
        Type = type;
        Data = data;
    }

    public ObjectKind TargetObject { get; init; } = ObjectKind.Player;
    public PairChangeType Type { get; init; } = PairChangeType.None;
    public string Data { get; init; } = string.Empty;
}
