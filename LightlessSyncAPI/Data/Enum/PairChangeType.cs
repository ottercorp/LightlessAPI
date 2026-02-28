namespace LightlessSync.API.Data.Enum;

[Flags]
public enum PairChangeType
{
    None = 0,
    Glamourer = 1 << 0,
    CustomizePlus = 1 << 1,
    Heels = 1 << 2,
    Honorific = 1 << 3,
    Moodles = 1 << 4,
    PetNames = 1 << 5,
    Manipulation = 1 << 6,
    ModdedPaths = 1 << 7,
}

public static class PairChangeTypeExtensions
{
    private const PairChangeType VisualMask =
        PairChangeType.Glamourer
        | PairChangeType.CustomizePlus
        | PairChangeType.Heels
        | PairChangeType.Honorific
        | PairChangeType.Moodles
        | PairChangeType.PetNames
        | PairChangeType.Manipulation;

    public static bool IsVisualOnly(this PairChangeType types)
        => types != PairChangeType.None
        && (types & ~VisualMask) == PairChangeType.None;

    public static bool IsSingleVisualType(this PairChangeType type)
        => type is PairChangeType.Glamourer
            or PairChangeType.CustomizePlus
            or PairChangeType.Heels
            or PairChangeType.Honorific
            or PairChangeType.Moodles
            or PairChangeType.PetNames
            or PairChangeType.Manipulation;
}
