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
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.ProfilePicture == null
                        ? null
                        : src.ProfilePicture.BlobName));

        CreateMap<User, UserPreviewDto>()
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.ProfilePicture != null
                        ? src.ProfilePicture.BlobName
                        : null));

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        // Course
        CreateMap<Course, CoursePreviewDto>()
            .ForMember(
                dest => dest.Teacher,
                opt => opt.MapFrom(src => src.Teacher))
            .ForMember(
                dest => dest.StudentCount,
                opt => opt.MapFrom(src => src.CourseStudents.Count()));

        CreateMap<Course, CourseDto>()
            .ForMember(
                dest => dest.Teacher,
                opt => opt.MapFrom(src => src.Teacher))
            .ForMember(
                dest => dest.StudentCount,
                opt => opt.MapFrom(src => src.CourseStudents.Count()));
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();


        // Assignment
        CreateMap<Assignment, AssignmentDto>()
            .ForMember(dest => dest.CourseTitle,
                opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.CourseId,
                opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        CreateMap<Assignment, AssignmentPreviewDto>()
            .ForMember(dest => dest.CourseTitle,
                opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        CreateMap<CreateAssignmentDto, Assignment>();
        CreateMap<UpdateAssignmentDto, Assignment>();


        // Announcement
        CreateMap<Announcement, AnnouncementDto>()
            .ForMember(
                dest => dest.CourseTitle,
                opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(
                dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        CreateMap<CreateAnnouncementDto, Announcement>();
        CreateMap<UpdateAnnouncementDto, Announcement>();

        // Submission
        CreateMap<Submission, SubmissionDto>()
            .ForMember(dest => dest.AssignmentTitle,
                opt => opt.MapFrom(src => src.Assignment.Title))
            .ForMember(dest => dest.StudentFirstName,
                opt => opt.MapFrom(src => src.Student.FirstName))
            .ForMember(dest => dest.StudentLastName,
                opt => opt.MapFrom(src => src.Student.LastName))
            .ForMember(dest => dest.StudentProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.Student.ProfilePicture != null
                        ? src.Student.ProfilePicture.BlobName
                        : null))
            .ForMember(dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        
        CreateMap<Submission, SubmissionPreviewDto>()
            .ForMember(dest => dest.AssignmentTitle,
                opt => opt.MapFrom(src => src.Assignment.Title))
            .ForMember(dest => dest.StudentName,
                opt => opt.MapFrom(src =>
                    $"{src.Student.FirstName} {src.Student.LastName}"))
            .ForMember(dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        CreateMap<CreateSubmissionDto, Submission>();
        CreateMap<UpdateSubmissionDto, Submission>();

        // StudyFolder
        CreateMap<StudyFolder, StudyFolderDto>()
            .ForMember(
                dest => dest.CourseTitle,
                opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(
                dest => dest.ChildFolderCount,
                opt => opt.MapFrom(src => src.ChildFolders.Count))
            .ForMember(
                dest => dest.FileCount,
                opt => opt.MapFrom(src => src.Files.Count));
        CreateMap<CreateStudyFolderDto, StudyFolder>();
        CreateMap<UpdateStudyFolderDto, StudyFolder>();

        // StudyFile
        CreateMap<StudyFile, StudyFileDto>()
            .ForMember(
                dest => dest.FileSize,
                opt => opt.MapFrom(src => src.Size))
            .ForMember(
                dest => dest.UploadedByName,
                opt => opt.MapFrom(src =>
                    $"{src.UploadedBy.FirstName} {src.UploadedBy.LastName}"));
        CreateMap<UpdateStudyFileDto, StudyFile>();
    }
}