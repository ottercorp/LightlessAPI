using LightlessSync.API.Data;

namespace LightlessSync.API.Dto.User
{
    public record UserBlacklistDto(UserData BlacklistedUser, DateTime BlockedDate) : UserDto(BlacklistedUser);
}
