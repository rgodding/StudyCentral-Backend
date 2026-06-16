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

        await Clients.Caller.SendAsync("JoinedRoom", chatRoom);
    }
    
    public async Task SendMessage(
        Guid chatRoomId,
        SendChatMessageDto dto)
    {
        var currentUser = Context.User!.GetUser();

        var message = await _chatService.SendMessage(
            currentUser.Id,
            chatRoomId,
            dto);

        await Clients
            .Group(GetRoomGroupName(chatRoomId))
            .SendAsync("ReceiveMessage", message);
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
}