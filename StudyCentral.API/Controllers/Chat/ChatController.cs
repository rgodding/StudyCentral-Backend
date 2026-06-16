using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Models.DTOs.Chat.ChatRoom;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Chat;

[ApiController]
[Route("api/[controller]")]
public class ChatController : BaseController
{
    private readonly IChatService _chatService;
    
    public ChatController(IMapper mapper, IChatService chatService) : base(mapper)
    {
        _chatService = chatService;
    }

    [HttpGet("courses/{courseId:guid}/room")]
    public async Task<ActionResult<ChatRoomDto>> GetOrCreateCourseChatRoom(Guid courseId)
    {
        var chatRoom = await _chatService.GetOrCreateCourseChatRoom(CurrentUser.Id, courseId);
        
        return Ok(chatRoom);
    }

    [HttpGet("rooms/{chatRoomId:guid}/messages")]
    public async Task<ActionResult<List<ChatMessageDto>>> GetMessages(Guid chatRoomId)
    {
        var messages = await _chatService.GetMessages(
            CurrentUser.Id,
            chatRoomId
        );
        
        return Ok(messages);   
    }
    
    [HttpPost("rooms/{chatRoomId:guid}/messages")]
    public async Task<ActionResult<ChatMessageDto>> SendMessage(Guid chatRoomId, SendChatMessageDto dto)
    {
        var message = await _chatService.SendMessage(
            CurrentUser.Id,
            chatRoomId,
            dto
        );
        
        return Ok(message);
    }
}