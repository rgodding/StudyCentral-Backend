using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudyCentral.API.Hubs;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class ChatServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly ChatService _service;


    public ChatServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();

        var clientProxyMock = new Mock<IClientProxy>();
        var hubClientsMock = new Mock<IHubClients>();

        hubClientsMock
            .Setup(x => x.Group(It.IsAny<string>()))
            .Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<ChatHub>>();
        hubContextMock
            .Setup(x => x.Clients)
            .Returns(hubClientsMock.Object);

        _service = new ChatService(
            _dbContext,
            mapper,
            hubContextMock.Object
        );
    }

    [Fact]
    public async Task GetOrCreateCourseChatRoom_WhenCourseExists_CreatesAndReturnsChatRoom()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        var course = TestCourseFactory.Create(teacherId: teacher.Id);
        _dbContext.Users.AddRange(teacher);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        // Act
        var chatRoom = await _service.GetOrCreateCourseChatRoom(teacher.Id, course.Id);

        // Assert
        Assert.NotNull(chatRoom);
        Assert.Equal(course.Id, chatRoom.CourseId);
    }

    [Fact]
    public async Task GetOrCreateCourseChatRoom_WhenCourseAndChatRoomExists_ReturnsChatRoom()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        var course = TestCourseFactory.Create(teacherId: teacher.Id);
        var chatRoom = TestChatRoomFactory.Create(courseId: course.Id);
        _dbContext.Users.AddRange(teacher);
        _dbContext.ChatRooms.Add(chatRoom);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        // Act
        var returnedChatRoom = await _service.GetOrCreateCourseChatRoom(teacher.Id, course.Id);

        // Assert
        Assert.NotNull(returnedChatRoom);
        Assert.Equal(chatRoom.Id, returnedChatRoom.Id);
        Assert.Equal(course.Id, returnedChatRoom.CourseId);
        Assert.Equal(chatRoom.CreatedAt, returnedChatRoom.CreatedAt);
    }

    [Fact]
    public async Task GetOrCreateCourseChatRoom_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        _dbContext.Users.Add(teacher);
        await _dbContext.SaveChangesAsync();

        var courseId = Guid.NewGuid();

        // Act + Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetOrCreateCourseChatRoom(teacher.Id, courseId));
    }

    [Fact]
    public async Task GetMessages_WhenUserHasAccess_ReturnsMessages()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        var course = TestCourseFactory.Create(teacherId: teacher.Id);
        var chatRoom = TestChatRoomFactory.Create(courseId: course.Id);
        var member = TestChatRoomMemberFactory.Create(
            userId: teacher.Id,
            chatRoomId: chatRoom.Id);

        var message = TestChatMessageFactory.Create(
            chatRoomId: chatRoom.Id,
            senderId: teacher.Id);

        _dbContext.Users.Add(teacher);
        _dbContext.Courses.Add(course);
        _dbContext.ChatRooms.Add(chatRoom);
        _dbContext.ChatRoomMembers.Add(member);
        _dbContext.ChatMessages.Add(message);
        await _dbContext.SaveChangesAsync();

        // Act
        var messages = await _service.GetMessages(teacher.Id, chatRoom.Id);

        // Assert
        Assert.NotNull(messages);
        Assert.Single(messages);
        Assert.Equal(message.Id, messages[0].Id);
        Assert.Equal(message.Content, messages[0].Content);
    }

    [Fact]
    public async Task GetMessages_WhenUserDoesNotHaveAccess_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        var outsider = TestUserFactory.Create();
        var course = TestCourseFactory.Create(teacherId: teacher.Id);
        var chatRoom = TestChatRoomFactory.Create(courseId: course.Id);

        _dbContext.Users.AddRange(teacher, outsider);
        _dbContext.Courses.Add(course);
        _dbContext.ChatRooms.Add(chatRoom);
        await _dbContext.SaveChangesAsync();

        // Act + Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.GetMessages(outsider.Id, chatRoom.Id));
    }

    [Fact]
    public async Task SendMessage_WhenUserHasAccess_CreatesAndReturnsMessage()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        var course = TestCourseFactory.Create(teacherId: teacher.Id);
        var chatRoom = TestChatRoomFactory.Create(courseId: course.Id);
        var member = TestChatRoomMemberFactory.Create(
            userId: teacher.Id,
            chatRoomId: chatRoom.Id);

        var dto = new SendChatMessageDto
        {
            Content = "Test chat message"
        };

        _dbContext.Users.Add(teacher);
        _dbContext.Courses.Add(course);
        _dbContext.ChatRooms.Add(chatRoom);
        _dbContext.ChatRoomMembers.Add(member);
        await _dbContext.SaveChangesAsync();

        // Act
        var message = await _service.SendMessage(teacher.Id, chatRoom.Id, dto);

        // Assert
        Assert.NotNull(message);
        Assert.Equal(chatRoom.Id, message.ChatRoomId);
        Assert.Equal(teacher.Id, message.SenderId);
        Assert.Equal(dto.Content, message.Content);

        var messageExists = await _dbContext.ChatMessages
            .AnyAsync(m => m.Id == message.Id);

        Assert.True(messageExists);
    }

    [Fact]
    public async Task SendMessage_WhenUserDoesNotHaveAccess_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var teacher = TestUserFactory.Create();
        var outsider = TestUserFactory.Create();
        var course = TestCourseFactory.Create(teacherId: teacher.Id);
        var chatRoom = TestChatRoomFactory.Create(courseId: course.Id);

        var dto = new SendChatMessageDto
        {
            Content = "Unauthorized message"
        };

        _dbContext.Users.AddRange(teacher, outsider);
        _dbContext.Courses.Add(course);
        _dbContext.ChatRooms.Add(chatRoom);
        await _dbContext.SaveChangesAsync();

        // Act + Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.SendMessage(outsider.Id, chatRoom.Id, dto));
    }
}