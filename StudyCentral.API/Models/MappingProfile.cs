using AutoMapper;
using StudyCentral.API.Models.Dtos.Announcements;
using StudyCentral.API.Models.Dtos.Assignments;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Models.Dtos.StudyFiles;
using StudyCentral.API.Models.Dtos.Submissions;
using StudyCentral.API.Models.Dtos.Users;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        // User Mappings
        CreateMap<User, UserDto>();
        
        // Course Mappings
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();

        // Announcement Mappings
        CreateMap<Announcement, AnnouncementDto>();
        CreateMap<CreateAnnouncementDto, Announcement>();
        
        // Assignment Mappings
        CreateMap<Assignment, AssignmentDto>();
        CreateMap<CreateAssignmentDto, Assignment>();
        
        // Submission Mappings
        CreateMap<Submission, SubmissionDto>();
        CreateMap<CreateSubmissionDto, Submission>();
        
        // StudyFile Mappings
        CreateMap<StudyFile, StudyFileDto>();
        
    }
}