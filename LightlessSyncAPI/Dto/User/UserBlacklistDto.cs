using LightlessSync.API.Data;

namespace LightlessSync.API.Dto.User
{
    public record UserBlacklistDto(UserData blacklistedUser, DateTime blockedDate) : UserDto(blacklistedUser);
}
