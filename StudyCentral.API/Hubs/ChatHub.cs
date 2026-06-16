using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace StudyCentral.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinRoom(Guid chatRoomId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            GetRoomGroupName(chatRoomId));
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            GetRoomGroupName(chatRoomId));
    }

    public static string GetRoomGroupName(Guid chatRoomId)
    {
        return $"room_{chatRoomId}";
    }
    
}