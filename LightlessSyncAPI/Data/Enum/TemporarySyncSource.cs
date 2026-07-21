namespace LightlessSync.API.Data.Enum;

[Flags]
public enum TemporarySyncSource : byte
{
    None = 0,
    Party = 1 << 0,
}
