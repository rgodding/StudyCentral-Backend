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
        CreateUserMappings();
        CreateCourseMappings();
        CreateAssignmentMappings();
        CreateSubmissionMappings();
    }

    private void CreateUserMappings()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>();
    }
    private void CreateCourseMappings()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();
    }
    private void CreateAssignmentMappings()
    {
        CreateMap<Assignment, AssignmentDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CourseId,
                opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        CreateMap<Assignment, AssignmentDto>();
        CreateMap<CreateAssignmentDto, Assignment>();
    }
    private void CreateSubmissionMappings()
    {
        CreateMap<Submission, SubmissionDto>()
            // Assignment
            .ForMember(dest => dest.AssignmentId,
                opt => opt.MapFrom(src => src.AssignmentId))
            .ForMember(dest => dest.AssignmentName,
                opt => opt.MapFrom(src => src.Assignment.Name))

            // Student
            .ForMember(dest => dest.StudentId,
                opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.StudentFirstName,
                opt => opt.MapFrom(src => src.Student.FirstName))
            .ForMember(dest => dest.StudentLastName,
                opt => opt.MapFrom(src => src.Student.LastName))
            .ForMember(dest => dest.StudentProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Student.ProfilePicture != null
                        ? src.Student.ProfilePicture.BlobName
                        : null))

            // Files
            .ForMember(dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count))

            // Dates
            .ForMember(dest => dest.SubmittedAt,
                opt => opt.MapFrom(src => src.SubmittedAt))
            .ForMember(dest => dest.GradedAt,
                opt => opt.MapFrom(src => src.GradedAt))
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<CreateSubmissionDto, Submission>();
    }
}