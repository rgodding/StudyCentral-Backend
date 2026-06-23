using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Models.DTOs.Chat.ChatRoomMember;
using StudyCentral.API.Services;

namespace StudyCentral.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    private static readonly ConcurrentDictionary<string, ChatConnectionInfo> Connections = new();

    private class ChatConnectionInfo
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public Guid ChatRoomId { get; set; }
    }

    public async Task JoinCourseRoom(Guid courseId)
    {
        var currentUser = Context.User!.GetUser();

        var chatRoom = await _chatService.GetOrCreateCourseChatRoom(
            currentUser.Id,
            courseId);

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            GetRoomGroupName(chatRoom.Id));

        var chatUser = await _chatService.GetCurrentUser(currentUser.Id);

        Connections[Context.ConnectionId] = new ChatConnectionInfo
        {
            UserId = chatUser.Id,
            Name = chatUser.Name,
            ProfilePictureUrl = chatUser.ProfilePictureUrl,
            ChatRoomId = chatRoom.Id
        };

        var messages = await _chatService.GetMessages(
            currentUser.Id,
            chatRoom.Id);

        await _chatService.MarkChatRoomAsSeen(
            currentUser.Id,
            chatRoom.Id);

        await Clients.Caller.SendAsync("JoinedRoom", chatRoom);
        await Clients.Caller.SendAsync("ChatMessagesLoaded", messages);

        await SendRoomUsers(chatRoom.Id);
        await NotifyChatOverviewChanged(chatRoom.Id);
    }

    public async Task SendMessage(Guid chatRoomId, SendChatMessageDto dto)
    {
        var currentUser = Context.User!.GetUser();

        var message = await _chatService.SendMessage(
            currentUser.Id,
            chatRoomId,
            dto);

        await MarkActiveRoomUsersAsSeen(chatRoomId);

        await Clients
            .Group(GetRoomGroupName(chatRoomId))
            .SendAsync("ReceiveMessage", message);

        await NotifyChatOverviewChanged(chatRoomId);
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            GetRoomGroupName(chatRoomId));

        Connections.TryRemove(Context.ConnectionId, out _);

        await SendRoomUsers(chatRoomId);
        await NotifyChatOverviewChanged(chatRoomId);
    }

    public async Task GetCourseChatRooms()
    {
        var currentUser = Context.User!.GetUser();

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            GetUserChatOverviewGroupName(currentUser.Id));

        var chatRooms = await _chatService.GetCourseChatRooms(currentUser.Id);

        await Clients.Caller.SendAsync("CourseChatRoomsLoaded", chatRooms);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Connections.TryRemove(Context.ConnectionId, out var connectionInfo))
        {
            await SendRoomUsers(connectionInfo.ChatRoomId);
            await NotifyChatOverviewChanged(connectionInfo.ChatRoomId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    // ----------------
    // HELPER METHODS
    // ----------------

    private async Task MarkActiveRoomUsersAsSeen(Guid chatRoomId)
    {
        var activeRoomUserIds = Connections.Values
            .Where(c => c.ChatRoomId == chatRoomId)
            .Select(c => c.UserId)
            .Distinct()
            .ToList();

        foreach (var userId in activeRoomUserIds)
        {
            await _chatService.MarkChatRoomAsSeen(userId, chatRoomId);
        }
    }

    private async Task NotifyChatOverviewChanged(Guid chatRoomId)
    {
        var memberIds = await _chatService.GetChatRoomMemberIds(chatRoomId);

        var overviewGroups = memberIds
            .Select(GetUserChatOverviewGroupName)
            .ToList();

        await Clients
            .Groups(overviewGroups)
            .SendAsync("CourseChatRoomsChanged");
    }

    private async Task SendRoomUsers(Guid chatRoomId)
    {
        var users = Connections.Values
            .Where(c => c.ChatRoomId == chatRoomId)
            .GroupBy(c => c.UserId)
            .Select(g => new ChatOnlineUserDto
            {
                UserId = g.Key,
                Name = g.First().Name,
                ProfilePictureUrl = g.First().ProfilePictureUrl
            })
            .ToList();

        await Clients
            .Group(GetRoomGroupName(chatRoomId))
            .SendAsync("RoomUsersChanged", users);
    }

    private static string GetUserChatOverviewGroupName(Guid userId)
    {
        return $"user-{userId}-chat-overview";
    }

    private static string GetRoomGroupName(Guid chatRoomId)
    {
        return $"chat-room-{chatRoomId}";
    }
}