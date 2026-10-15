using LightlessSync.API.Data.Enum;

namespace LightlessSync.API.Data.Extensions;

public static class GroupPermissionsExtensions
{
    public static bool IsDisableInvites(this GroupPermissions perm)
    {
        return (perm & GroupPermissions.DisableInvites) == GroupPermissions.DisableInvites;
    }

    public static bool IsPreferDisableAnimations(this GroupPermissions perm)
    {
        return (perm & GroupPermissions.PreferDisableAnimations) == GroupPermissions.PreferDisableAnimations;
    }

    public static bool IsPreferDisableSounds(this GroupPermissions perm)
    {
        return (perm & GroupPermissions.PreferDisableSounds) == GroupPermissions.PreferDisableSounds;
    }

    public static bool IsPreferDisableVFX(this GroupPermissions perm)
    {
        return (perm & GroupPermissions.PreferDisableVFX) == GroupPermissions.PreferDisableVFX;
    }

    public static bool IsDisableChat(this GroupPermissions perm)
    {
        return (perm & GroupPermissions.DisableChat) == GroupPermissions.DisableChat;
    }

    public static void SetDisableInvites(this ref GroupPermissions perm, bool set)
    {
        if (set) perm |= GroupPermissions.DisableInvites;
        else perm &= ~GroupPermissions.DisableInvites;
    }

    public static void SetPreferDisableAnimations(this ref GroupPermissions perm, bool set)
    {
        if (set) perm |= GroupPermissions.PreferDisableAnimations;
        else perm &= ~GroupPermissions.PreferDisableAnimations;
    }

    public static void SetPreferDisableSounds(this ref GroupPermissions perm, bool set)
    {
        if (set) perm |= GroupPermissions.PreferDisableSounds;
        else perm &= ~GroupPermissions.PreferDisableSounds;
    }

    public static void SetPreferDisableVFX(this ref GroupPermissions perm, bool set)
    {
        if (set) perm |= GroupPermissions.PreferDisableVFX;
        else perm &= ~GroupPermissions.PreferDisableVFX;
    }

    public static void SetDisableChat(this ref GroupPermissions perm, bool set)
    {
        if (set) perm |= GroupPermissions.DisableChat;
        else perm &= ~GroupPermissions.DisableChat;
    }
}