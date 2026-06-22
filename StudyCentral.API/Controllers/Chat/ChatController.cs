using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Models.DTOs.Chat.ChatRoom;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Chat;

[ApiController]
[Tags("Chat")]
[Route("api/chat")]
public class ChatController : BaseController
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("courses/{courseId:guid}/room")]
    [SwaggerOperation(
        Summary = "Get or create course chat room",
        Description = "Gets or creates the chat room for a course."
    )]
    public async Task<ActionResult<ChatRoomDto>> GetOrCreateCourseChatRoom(Guid courseId)
    {
        var chatRoom = await _chatService.GetOrCreateCourseChatRoom(CurrentUserId, courseId);

        return Ok(chatRoom);
    }

    [HttpGet("rooms/{chatRoomId:guid}/messages")]
    [SwaggerOperation(
        Summary = "Get messages",
        Description = "Gets messages from one chat room."
    )]
    public async Task<ActionResult<List<ChatMessageDto>>> GetMessages(Guid chatRoomId)
    {
        var messages = await _chatService.GetMessages(
            CurrentUserId,
            chatRoomId
        );

        return Ok(messages);   
    }

    [HttpPost("rooms/{chatRoomId:guid}/messages")]
    [SwaggerOperation(
        Summary = "Send message",
        Description = "Sends a message to one chat room."
    )]
    public async Task<ActionResult<ChatMessageDto>> SendMessage(Guid chatRoomId, SendChatMessageDto dto)
    {
        var message = await _chatService.SendMessage(
            CurrentUserId,
            chatRoomId,
            dto
        );

        return Ok(message);
    }
}