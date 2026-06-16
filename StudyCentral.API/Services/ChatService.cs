using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Hubs;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Models.DTOs.Chat.ChatRoom;
using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Services;

public interface IChatService
{
    Task<ChatRoomDto> GetOrCreateCourseChatRoom(Guid currentUserId, Guid courseId);
    
    Task<List<ChatMessageDto>> GetMessages(Guid currentUserId, Guid chatRoomId);

    Task<ChatMessageDto> SendMessage(Guid currentUserId, Guid chatRoomId, SendChatMessageDto dto);
}

public class ChatService : IChatService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub> _chatHub;

    public ChatService(StudyDbContext dbContext, IMapper mapper, IHubContext<ChatHub> chatHub)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _chatHub = chatHub;
    }

    // --------------------
    // METHODS
    // --------------------
    public async Task<ChatRoomDto> GetOrCreateCourseChatRoom(Guid currentUserId, Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        
        if (course == null)
            throw new KeyNotFoundException("Course not found");
        
        var hasAccess = course.TeacherId == currentUserId ||
                        course.CourseStudents.Any(cs => cs.StudentId == currentUserId);
        
        if (!hasAccess)
            throw new UnauthorizedAccessException("User does not have access to this course");
        
        var chatRoom = await _dbContext.ChatRooms
            .Include(cr => cr.Course)
            .Include(cr => cr.Members)
            .Include(cr => cr.Messages)
            .FirstOrDefaultAsync(cr =>
                cr.Type == ChatRoomType.Course &&
                cr.CourseId == courseId);

        if (chatRoom == null)
        {
            chatRoom = new ChatRoom
            {
                Name = $"{course.Name} Chat",
                Type = ChatRoomType.Course,
                CourseId = courseId
            };
            
            _dbContext.ChatRooms.Add(chatRoom);

            if (course.TeacherId != null)
            {
                chatRoom.Members.Add(new ChatRoomMember
                {
                    UserId = course.TeacherId.Value,
                    ChatRoomId = chatRoom.Id
                });
            }

            foreach (var student in course.CourseStudents)
            {
                chatRoom.Members.Add(new ChatRoomMember
                {
                    UserId = student.StudentId,
                    ChatRoomId = chatRoom.Id
                });
            }
            
            await _dbContext.SaveChangesAsync();
        }
        
        return _mapper.Map<ChatRoomDto>(chatRoom);
    }

    public async Task<List<ChatMessageDto>> GetMessages(Guid currentUserId, Guid chatRoomId)
    {
        await VerifyChatRoomAccess(currentUserId, chatRoomId);
        
        var messages = await _dbContext.ChatMessages
            .Include(cm => cm.Sender)
            .Where(cm => cm.ChatRoomId == chatRoomId)
            .OrderBy(cm => cm.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<ChatMessageDto>>(messages);
    }

    public async Task<ChatMessageDto> SendMessage(Guid currentUserId, Guid chatRoomId, SendChatMessageDto dto)
    {
        await VerifyChatRoomAccess(currentUserId, chatRoomId);
        
        var message = _mapper.Map<ChatMessage>(dto);
        message.ChatRoomId = chatRoomId;
        message.SenderId = currentUserId;
        
        _dbContext.ChatMessages.Add(message);
        await _dbContext.SaveChangesAsync();
        
        var createdMessage = await _dbContext.ChatMessages
            .Include(m => m.Sender)
            .FirstAsync(m => m.Id == message.Id);

        var messageDto = _mapper.Map<ChatMessageDto>(createdMessage);

        return messageDto;
    }
    
    // --------------------
    // HELPER METHODS
    // --------------------
    private async Task VerifyChatRoomAccess(Guid currentUserId, Guid chatRoomId)
    {
        var hasAccess = await _dbContext.ChatRoomMembers
            .AnyAsync(m =>
                m.ChatRoomId == chatRoomId &&
                m.UserId == currentUserId);

        if (!hasAccess)
            throw new UnauthorizedAccessException("You do not have access to this chat room");
    }
}