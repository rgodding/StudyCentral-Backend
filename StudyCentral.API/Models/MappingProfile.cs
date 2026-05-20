using AutoMapper;
using StudyCentral.API.Models.ApiModels.AuthModels;
using StudyCentral.API.Models.DtoModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        
        
        CreateMap<Course, CourseDto>();
        CreateMap<Assignment, AssignmentDto>();
        CreateMap<StudyFile, StudyFileDto>();
        CreateMap<Chat, ChatDto>();
        CreateMap<Message, MessageDto>();
        CreateMap<Submission, SubmissionDto>();
        CreateMap<Notification, NotificationDto>();

        CreateMap<SignUpRequestModel, User>();
            /*
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Avatar, opt => opt.Ignore())
            .ForMember(dest => dest.Courses, opt => opt.Ignore())
            .ForMember(dest => dest.Submissions, opt => opt.Ignore())
            .ForMember(dest => dest.Chats, opt => opt.Ignore())
            .ForMember(dest => dest.Notifications, opt => opt.Ignore())
            .ForMember(dest => dest.Messages, opt => opt.Ignore());
            */
        
        CreateMap<User, SignUpResponseModel>();

    }
}