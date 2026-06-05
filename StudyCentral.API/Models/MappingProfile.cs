using AutoMapper;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
        
        // Course
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();
    }
}