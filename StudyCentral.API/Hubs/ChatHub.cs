using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
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

    private static readonly Dictionary<string, Guid> ConnectionRooms = new();

    public async Task JoinCourseRoom(Guid courseId)
    {
        var currentUser = Context.User!.GetUser();

        var chatRoom = await _chatService.GetOrCreateCourseChatRoom(
            currentUser.Id,
            courseId);

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            GetRoomGroupName(chatRoom.Id));

        ConnectionRooms[Context.ConnectionId] = chatRoom.Id;

        var messages = await _chatService.GetMessages(
            currentUser.Id,
            chatRoom.Id);

        await _chatService.MarkChatRoomAsSeen(
            currentUser.Id,
            chatRoom.Id);

        await Clients.Caller.SendAsync("JoinedRoom", chatRoom);
        await Clients.Caller.SendAsync("ChatMessagesLoaded", messages);
    }

    public async Task SendMessage(Guid chatRoomId, SendChatMessageDto dto)
    {
        var currentUser = Context.User!.GetUser();

        var message = await _chatService.SendMessage(
            currentUser.Id,
            chatRoomId,
            dto);

        await Clients
            .Group(GetRoomGroupName(chatRoomId))
            .SendAsync("ReceiveMessage", message);

        var memberIds = await _chatService.GetChatRoomMemberIds(chatRoomId);

        var overviewGroups = memberIds
            .Select(GetUserChatOverviewGroupName)
            .ToList();

        await Clients
            .Groups(overviewGroups)
            .SendAsync("CourseChatRoomsChanged");
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            GetRoomGroupName(chatRoomId));
    }

    public static string GetRoomGroupName(Guid chatRoomId)
    {
        return $"chat-room-{chatRoomId}";
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (ConnectionRooms.TryGetValue(Context.ConnectionId, out var chatRoomId))
        {
            ConnectionRooms.Remove(Context.ConnectionId);

            var currentUser = Context.User?.GetUser();

            if (currentUser != null)
            {
                await Clients
                    .Group(GetRoomGroupName(chatRoomId))
                    .SendAsync("UserDisconnected", new
                    {
                        chatRoomId,
                        userId = currentUser.Id,
                        message = $"{currentUser.Email} disconnected from chat.",
                        disconnectedAt = DateTime.UtcNow
                    });
            }
        }

        await base.OnDisconnectedAsync(exception);
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

    private static string GetUserChatOverviewGroupName(Guid userId)
    {
        return $"user-{userId}-chat-overview";
    }
}