using AutoMapper;
using StudyCentral.API.Models.DtoModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<User, UserDto>();
        CreateMap<Course, CourseDto>();
        CreateMap<Assignment, AssignmentDto>();
        CreateMap<StudyFile, StudyFileDto>();
        CreateMap<Chat, ChatDto>();
        CreateMap<Message, MessageDto>();
        CreateMap<Submission, SubmissionDto>();
        CreateMap<Notification, NotificationDto>();
        
    }
}