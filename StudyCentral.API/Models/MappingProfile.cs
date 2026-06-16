using AutoMapper;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.Chat.ChatMessage;
using StudyCentral.API.Models.DTOs.Chat.ChatRoom;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Chat;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateUserMappings();
        CreateCourseMappings();
        CreateAssignmentMappings();
        CreateSubmissionMappings();
        CreateAnnouncementMappings();
        CreateStudyFolderMappings();
        CreateStudyFileMappings();
        CreateChatRoomMappings();
        CreateChatMessageMappings();
    }

    private void CreateUserMappings()
    {
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.ProfilePictureUrl,
                opt => opt.MapFrom(src =>
                    src.ProfilePicture != null
                        ? src.ProfilePicture.BlobName
                        : null));

        CreateMap<CreateUserDto, User>();
    }
    private void CreateCourseMappings()
    {
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.StudentCount,
                opt => opt.MapFrom(src => src.CourseStudents.Count));

        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();
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
                opt => opt.MapFrom(src => src.StudyFiles.Count));
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
                opt => opt.MapFrom(src => src.StudyFiles.Count))

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
    private void CreateAnnouncementMappings()
    {
        CreateMap<Announcement, AnnouncementDto>()
            .ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(
                dest => dest.FileCount,
                opt => opt.MapFrom(src => src.StudyFiles.Count));
        
        CreateMap<CreateAnnouncementDto, Announcement>();
    }
    private void CreateStudyFolderMappings()
    {
        CreateMap<StudyFolder, StudyFolderDto>()
            .ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(
                dest => dest.ChildFolderCount,
                opt => opt.MapFrom(src => src.ChildFolders.Count))
            .ForMember(
                dest => dest.FileCount,
                opt => opt.MapFrom(src => src.StudyFiles.Count));

        CreateMap<CreateStudyFolderDto, StudyFolder>();
    }
    private void CreateStudyFileMappings()
    {
        CreateMap<StudyFile, StudyFileDto>()
            .ForMember(
                dest => dest.FileSize,
                opt => opt.MapFrom(src => src.Size))
            .ForMember(
                dest => dest.UploadedByName,
                opt => opt.MapFrom(src =>
                    $"{src.UploadedBy.FirstName} {src.UploadedBy.LastName}".Trim()))
            .ForMember(
                dest => dest.OwnerType,
                opt => opt.MapFrom(src =>
                    src.StudyFolderId != null ? FileOwnerType.Folder :
                    src.AssignmentId != null ? FileOwnerType.Assignment :
                    src.AnnouncementId != null ? FileOwnerType.Announcement :
                    src.SubmissionId != null ? FileOwnerType.Submission :
                    (FileOwnerType?)null))
            .ForMember(
                dest => dest.OwnerId,
                opt => opt.MapFrom(src =>
                    src.StudyFolderId ??
                    src.AssignmentId ??
                    src.AnnouncementId ??
                    src.SubmissionId));
    }

    private void CreateChatRoomMappings()
    {
        CreateMap<ChatRoom, ChatRoomDto>()
            .ForMember(dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null))
            .ForMember(dest => dest.MemberCount,
                opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.LastMessagePreview,
                opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.Content)
                    .FirstOrDefault()))
            .ForMember(dest => dest.LastMessageAt,
                opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => (DateTime?)m.CreatedAt)
                    .FirstOrDefault()));
    }
    
    private void CreateChatMessageMappings()
    {
        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(dest => dest.SenderName,
                opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"));
    }
}