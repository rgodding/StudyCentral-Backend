using AutoMapper;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Models.Dtos.Users;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserPreviewDto>();
        
        // Course Mappings
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();

    }
}