using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Hubs;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Models.DTOs.Chat.ChatRoom;
using StudyCentral.API.Models.DTOs.Chat.ChatRoomMember;
using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Services;

public interface IChatService
{
    Task<ChatCurrentUserDto> GetCurrentUser(Guid currentUserId);

    Task<List<ChatRoomDto>> GetCourseChatRooms(Guid currentUserId);

    Task<ChatRoomDto> GetOrCreateCourseChatRoom(Guid currentUserId, Guid courseId);

    Task<List<Guid>> GetChatRoomMemberIds(Guid chatRoomId);

    Task<List<ChatMessageDto>> GetMessages(Guid currentUserId, Guid chatRoomId);

    Task<ChatMessageDto> SendMessage(Guid currentUserId, Guid chatRoomId, SendChatMessageDto dto);

    Task MarkChatRoomAsSeen(Guid currentUserId, Guid chatRoomId);
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
    public async Task<ChatCurrentUserDto> GetCurrentUser(Guid currentUserId)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == currentUserId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        var name = $"{user.FirstName} {user.LastName}".Trim();

        return new ChatCurrentUserDto
        {
            Id = user.Id,
            Name = string.IsNullOrWhiteSpace(name)
                ? user.Email
                : name,
            ProfilePictureUrl = user.ProfilePicture?.BlobName
        };
    }

    public async Task<List<ChatRoomDto>> GetCourseChatRooms(Guid currentUserId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .Where(c =>
                c.TeacherId == currentUserId ||
                c.CourseStudents.Any(cs => cs.StudentId == currentUserId))
            .ToListAsync();

        var chatRooms = new List<ChatRoomDto>();

        foreach (var course in courses)
        {
            var chatRoomDto = await GetOrCreateCourseChatRoom(
                currentUserId,
                course.Id);

            var member = await _dbContext.ChatRoomMembers
                .FirstOrDefaultAsync(m =>
                    m.ChatRoomId == chatRoomDto.Id &&
                    m.UserId == currentUserId);

            var unreadCount = await _dbContext.ChatMessages
                .CountAsync(m =>
                    m.ChatRoomId == chatRoomDto.Id &&
                    m.SenderId != currentUserId &&
                    (member == null ||
                     member.LastSeenAt == null ||
                     m.CreatedAt > member.LastSeenAt));

            chatRoomDto.UnreadCount = unreadCount;

            chatRooms.Add(chatRoomDto);
        }

        return chatRooms
            .OrderByDescending(cr => cr.LastMessageAt ?? cr.CreatedAt)
            .ToList();
    }

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

    public async Task<List<Guid>> GetChatRoomMemberIds(Guid chatRoomId)
    {
        return await _dbContext.ChatRoomMembers
            .Where(m => m.ChatRoomId == chatRoomId)
            .Select(m => m.UserId)
            .ToListAsync();
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

    public async Task MarkChatRoomAsSeen(Guid currentUserId, Guid chatRoomId)
    {
        var member = await _dbContext.ChatRoomMembers
            .FirstOrDefaultAsync(m =>
                m.ChatRoomId == chatRoomId &&
                m.UserId == currentUserId);

        if (member == null)
            throw new UnauthorizedAccessException("You do not have access to this chat room");

        member.LastSeenAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
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