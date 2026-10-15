using LightlessSync.API.Data.Enum;

namespace LightlessSync.API.Data.Extensions;

public static class GroupUserPermissionsExtensions
{
    public static bool IsDisableAnimations(this GroupUserPreferredPermissions perm)
    {
        return (perm & GroupUserPreferredPermissions.DisableAnimations) == GroupUserPreferredPermissions.DisableAnimations;
    }

    public static bool IsDisableSounds(this GroupUserPreferredPermissions perm)
    {
        return (perm & GroupUserPreferredPermissions.DisableSounds) == GroupUserPreferredPermissions.DisableSounds;
    }

    public static bool IsDisableVFX(this GroupUserPreferredPermissions perm)
    {
        return (perm & GroupUserPreferredPermissions.DisableVFX) == GroupUserPreferredPermissions.DisableVFX;
    }

    public static bool IsPaused(this GroupUserPreferredPermissions perm)
    {
        return (perm & GroupUserPreferredPermissions.Paused) == GroupUserPreferredPermissions.Paused;
    }

    public static void SetDisableAnimations(this ref GroupUserPreferredPermissions perm, bool set)
    {
        if (set) perm |= GroupUserPreferredPermissions.DisableAnimations;
        else perm &= ~GroupUserPreferredPermissions.DisableAnimations;
    }

    public static void SetDisableSounds(this ref GroupUserPreferredPermissions perm, bool set)
    {
        if (set) perm |= GroupUserPreferredPermissions.DisableSounds;
        else perm &= ~GroupUserPreferredPermissions.DisableSounds;
    }

    public static void SetDisableVFX(this ref GroupUserPreferredPermissions perm, bool set)
    {
        if (set) perm |= GroupUserPreferredPermissions.DisableVFX;
        else perm &= ~GroupUserPreferredPermissions.DisableVFX;
    }

    public static void SetPaused(this ref GroupUserPreferredPermissions perm, bool set)
    {
        if (set) perm |= GroupUserPreferredPermissions.Paused;
        else perm &= ~GroupUserPreferredPermissions.Paused;
    }
}