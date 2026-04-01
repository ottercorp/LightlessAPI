using LightlessSync.API.Data;
using LightlessSync.API.Data.Enum;
using LightlessSync.API.Dto;
using LightlessSync.API.Dto.Chat;
using LightlessSync.API.Dto.CharaData;
using LightlessSync.API.Dto.Group;
using LightlessSync.API.Dto.User;

namespace LightlessSync.API.SignalR;

public interface ILightlessHub
{
    const int ApiVersion = 35;
    const string Path = "/lightless";

    Task<bool> CheckClientHealth();

    Task Client_DownloadReady(Guid requestId);
    Task Client_GroupChangePermissions(GroupPermissionDto groupPermission);
    Task Client_GroupDelete(GroupDto groupDto);
    Task Client_GroupPairChangeUserInfo(GroupPairUserInfoDto userInfo);
    Task Client_GroupPairJoined(GroupPairFullInfoDto groupPairInfoDto);
    Task Client_GroupPairLeft(GroupPairDto groupPairDto);
    Task Client_GroupSendFullInfo(GroupFullInfoDto groupInfo);
    Task Client_GroupSendProfile(GroupProfileDto groupInfo);
    Task Client_GroupSendInfo(GroupInfoDto groupInfo);
    Task Client_ReceiveServerMessage(MessageSeverity messageSeverity, string message);
    Task Client_ReceiveBroadcastPairRequest(UserPairNotificationDto dto);
    Task Client_UpdateSystemInfo(SystemInfoDto systemInfo);
    Task Client_UserAddClientPair(UserPairDto dto);
    Task Client_UserReceiveCharacterData(OnlineUserCharaDataDto dataDto);
    Task Client_PairReceiveVisualSingle(PairInboundDto<PairVisualDeltaDto> dataDto);
    Task Client_PairReceiveVisualDelta(PairInboundDto<PairVisualDeltaDto> dataDto);
    Task Client_PairReceiveModDelta(OnlineUserModDataDto dataDto);
    Task Client_UserReceiveUploadStatus(UserDto dto);
    Task Client_UserRemoveClientPair(UserDto dto);
    Task Client_UserSendOffline(UserDto dto);
    Task Client_UserSendOnline(OnlineUserIdentDto dto);
    Task Client_UserUpdateOtherPairPermissions(UserPermissionsDto dto);
    Task Client_UpdateUserIndividualPairStatusDto(UserIndividualPairStatusDto dto);
    Task Client_UserUpdateProfile(UserDto dto);
    Task Client_UserUpdateSelfPairPermissions(UserPermissionsDto dto);
    Task Client_UserUpdateDefaultPermissions(DefaultPermissionsDto dto);
    Task Client_GroupChangeUserPairPermissions(GroupPairUserPermissionDto dto);
    Task Client_GposeLobbyJoin(UserData userData);
    Task Client_GposeLobbyLeave(UserData userData);
    Task Client_GposeLobbyPushCharacterData(CharaDataDownloadDto charaDownloadDto);
    Task Client_GposeLobbyPushPoseData(UserData userData, PoseData poseData);
    Task Client_GposeLobbyPushWorldData(UserData userData, WorldData worldData);
    Task Client_ChatReceive(ChatMessageDto message);
    Task Client_SendLocationToClient(LocationDto locationDto, DateTimeOffset expireAt);

    Task<ConnectionDto> GetConnectionDto();
    Task<IReadOnlyList<ZoneChatChannelInfoDto>> GetZoneChatChannels();
    Task<IReadOnlyList<GroupChatChannelInfoDto>> GetGroupChatChannels();

    Task GroupBanUser(GroupPairDto dto, string reason);
    Task GroupChangeGroupPermissionState(GroupPermissionDto dto);
    Task GroupChangeOwnership(GroupPairDto groupPair);
    Task<bool> GroupChangePassword(GroupPasswordDto groupPassword);
    Task GroupClear(GroupDto group);
    Task GroupClearFinder(GroupDto group);
    Task<GroupJoinDto> GroupCreate();
    Task<List<string>> GroupCreateTempInvite(GroupDto group, int amount);
    Task GroupDelete(GroupDto group);
    Task<List<BannedGroupUserDto>> GroupGetBannedUsers(GroupDto group);
    Task<GroupJoinInfoDto> GroupJoin(GroupPasswordDto passwordedGroup);
    Task<bool> GroupJoinFinalize(GroupJoinDto passwordedGroup);
    Task<GroupJoinInfoDto> GroupJoinHashed(GroupJoinHashedDto dto);
    Task GroupLeave(GroupDto group);
    Task GroupRemoveUser(GroupPairDto groupPair);
    Task<GroupProfileDto> GroupGetProfile(GroupDto dto);
    Task GroupSetProfile(GroupProfileDto dto);
    Task GroupSetUserInfo(GroupPairUserInfoDto groupPair);
    Task<GroupPruneSettingsDto> GroupGetPruneSettings(GroupDto dto);
    Task GroupSetPruneSettings(GroupPruneSettingsDto dto);
    Task<List<GroupFullInfoDto>> GroupsGetAll();
    Task GroupUnbanUser(GroupPairDto groupPair);
    Task<int> GroupPrune(GroupDto group, int days, bool execute);
   

    Task UserAddPair(UserDto user);
    Task TryPairWithContentId(string otherCid);

    Task SetBroadcastStatus(bool enabled, GroupBroadcastRequestDto? groupDto = null);
    Task<bool> SetGroupBroadcastStatus(GroupBroadcastRequestDto dto);
    Task<List<GroupJoinDto>> GetBroadcastedGroups(List<BroadcastStatusInfoDto> broadcastEntries);
    Task<BroadcastStatusInfoDto?> IsUserBroadcasting(string hashedCid);
    Task<BroadcastStatusBatchDto?> AreUsersBroadcasting(List<string> hashedCids);
    Task<TimeSpan?> GetBroadcastTtl();

    Task UserDelete();
    Task<List<OnlineUserIdentDto>> UserGetOnlinePairs(CensusDataDto? censusDataDto);
    Task<List<UserFullPairDto>> UserGetPairedClients();
    Task<UserProfileDto> UserGetProfile(UserDto dto);
    Task<UserProfileDto?> UserGetLightfinderProfile(string hashedCid);
    Task<IReadOnlyList<UserBlacklistDto>> UserGetBlacklistedUsers();
    Task UserBlacklist(string targetUid);
    Task UserUnblacklist(string targetUid);
    Task PairPushVisualSingle(PairOutboundDto<PairVisualDeltaDto> dto);
    Task PairPushVisualDelta(PairOutboundDto<PairVisualDeltaDto> dto);
    Task PairPushModDelta(UserModDataMessageDto dto);

    Task UserPushData(UserCharaDataMessageDto dto);
    Task UserUpdateVanityColors(UserVanityColorsDto dto);
    Task UserRemovePair(UserDto userDto);
    Task UserSetProfile(UserProfileDto userDescription);
    Task UserUpdateDefaultPermissions(DefaultPermissionsDto defaultPermissionsDto);
    Task SetBulkPermissions(BulkPermissionsDto dto);

    Task<CharaDataFullDto?> CharaDataCreate();
    Task<CharaDataFullDto?> CharaDataUpdate(CharaDataUpdateDto updateDto);
    Task<bool> CharaDataDelete(string id);
    Task<CharaDataMetaInfoDto?> CharaDataGetMetainfo(string id);
    Task<CharaDataDownloadDto?> CharaDataDownload(string id);
    Task<List<CharaDataFullDto>> CharaDataGetOwn();
    Task<List<CharaDataMetaInfoDto>> CharaDataGetShared();
    Task<CharaDataFullDto?> CharaDataAttemptRestore(string id);

    Task<string> GposeLobbyCreate();
    Task<List<UserData>> GposeLobbyJoin(string lobbyId);
    Task<bool> GposeLobbyLeave();
    Task GposeLobbyPushCharacterData(CharaDataDownloadDto charaDownloadDto);
    Task GposeLobbyPushPoseData(PoseData poseData);
    Task GposeLobbyPushWorldData(WorldData worldData);
    Task UpdateChatPresence(ChatPresenceUpdateDto presence);
    Task SendChatMessage(ChatSendRequestDto request);
    Task ReportChatMessage(ChatReportSubmitDto request);
    Task SetChatParticipantMute(ChatParticipantMuteRequestDto request);
    
    Task UpdateLocation(LocationDto locationDto, bool offline);
    Task<(List<LocationWithTimeDto>, List<SharingStatusDto>)> RequestAllLocationInfo();
    Task<bool> ToggleLocationSharing(LocationSharingToggleDto dto);
    Task<List<GroupUserDto>> GroupGetUsers(GroupDto dto);
}
