using LightlessSync.API.Data;
using LightlessSync.API.Data.Enum;


namespace LightlessSync.API.Dto.Group
{
    public record GroupUserDto(
        GroupData Group,
        UserData User,
        GroupPairUserInfo GroupUserInfo,
        DateTime? JoinedOn,
        bool FromFinder
    );
}
