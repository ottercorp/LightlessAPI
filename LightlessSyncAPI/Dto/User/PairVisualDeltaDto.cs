using LightlessSync.API.Data.Enum;
using MessagePack;

namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public sealed record PairVisualDeltaDto
{
    public PairVisualDeltaDto()
    {
    }

    public PairVisualDeltaDto(List<PairVisualDeltaEntryDto> entries)
    {
        Entries = entries;
    }

    public List<PairVisualDeltaEntryDto> Entries { get; init; } = [];

    [IgnoreMember]
    public PairChangeType Types
    {
        get
        {
            var types = PairChangeType.None;
            foreach (var entry in Entries)
            {
                types |= entry.Type;
            }

            return types;
        }
    }

    public bool TryGetSingleEntry(out PairVisualDeltaEntryDto? entry)
    {
        if (Entries.Count == 1)
        {
            entry = Entries[0];
            return true;
        }

        entry = null;
        return false;
    }
}
