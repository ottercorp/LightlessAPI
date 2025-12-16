using System;
using System.Collections.Generic;
using LightlessSync.API.Data;
using LightlessSync.API.Dto.User;
using MessagePack;

namespace LightlessSync.API.Dto.Chat;

public enum ChatChannelType : byte
{
    Zone = 0,
    Group = 1
}

public enum ChatSenderKind : byte
{
    Anonymous = 0,
    IdentifiedUser = 1
}

[MessagePackObject]
public readonly record struct ChatChannelDescriptor
{
    [Key(0)] public ChatChannelType Type { get; init; }
    [Key(1)] public ushort WorldId { get; init; }
    [Key(2)] public ushort ZoneId { get; init; }
    [Key(3)] public string? CustomKey { get; init; }

    public ChatChannelDescriptor WithNormalizedCustomKey() =>
        this with { CustomKey = string.IsNullOrWhiteSpace(CustomKey) ? string.Empty : CustomKey };
}

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ChatPresenceUpdateDto(ChatChannelDescriptor Channel, ushort TerritoryId, bool IsActive);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ChatSendRequestDto(ChatChannelDescriptor Channel, string Message);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ChatSenderDescriptor(
    ChatSenderKind Kind,
    string Token,
    string? DisplayName,
    string? HashedCid,
    UserData? User,
    bool CanResolveProfile);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ChatMessageDto(
    ChatChannelDescriptor Channel,
    ChatSenderDescriptor Sender,
    string Message,
    DateTime SentAtUtc,
    string MessageId);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ChatReportSubmitDto(
    ChatChannelDescriptor Channel,
    string MessageId,
    string Reason,
    string? AdditionalContext);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ChatParticipantMuteRequestDto(
    ChatChannelDescriptor Channel,
    string Token,
    bool Mute);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct ZoneChatChannelInfoDto(
    ChatChannelDescriptor Channel,
    string DisplayName,
    IReadOnlyList<string> Territories);

[MessagePackObject(keyAsPropertyName: true)]
public readonly record struct GroupChatChannelInfoDto(
    ChatChannelDescriptor Channel,
    string DisplayName,
    string GroupId,
    bool IsOwner);
