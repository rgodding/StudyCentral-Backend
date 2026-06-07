using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Admin.Announcement;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAnnouncementService
{
    // CRUD
    Task<List<AnnouncementDto>> GetAll();
    Task<AnnouncementDto> GetById(Guid announcementId);
    Task<AnnouncementDto> Create(CreateAnnouncementDto dto);
    Task<AnnouncementDto> Update(Guid announcementId, UpdateAnnouncementDto dto);
    Task Delete(Guid announcementId);

    // Admin Methods
    Task<AnnouncementDto> AdminUpdateAnnouncement(Guid announcementId, AdminUpdateAnnouncementDto dto);
    Task<List<StudyFileDto>> GetFiles(Guid announcementId);

    // Teacher Methods
    Task<List<AnnouncementDto>> GetAnnouncementsByTeacherId(Guid teacherId);

    Task<AnnouncementDto> GetAnnouncementByTeacherId(
        Guid teacherId,
        Guid announcementId);

    Task<AnnouncementDto> CreateAnnouncementByTeacherId(
        Guid teacherId,
        CreateAnnouncementDto dto);

    Task<AnnouncementDto> UpdateAnnouncementByTeacherId(
        Guid teacherId,
        Guid announcementId,
        UpdateAnnouncementDto dto);

    Task DeleteAnnouncementByTeacherId(
        Guid teacherId,
        Guid announcementId);

    // Teacher Methods
    Task<List<AnnouncementDto>> GetAnnouncementsByCourseIdAndTeacherId(
        Guid teacherId,
        Guid courseId);

    // Teacher File Methods
    Task<StudyFileDto> UploadFileToAnnouncementByTeacherId(
        Guid teacherId,
        Guid announcementId,
        IFormFile file,
        string? altText = null);

    Task<List<StudyFileDto>> GetFilesByAnnouncementIdAndTeacherId(
        Guid teacherId,
        Guid announcementId);

    Task AttachFileToAnnouncementByTeacherId(
        Guid teacherId,
        Guid announcementId,
        Guid fileId);

    Task RemoveFileFromAnnouncementByTeacherId(
        Guid teacherId,
        Guid announcementId,
        Guid fileId);

    // Student Methods
    Task<List<AnnouncementDto>> GetAnnouncementsByStudentId(Guid studentId);

    Task<List<AnnouncementDto>> GetAnnouncementsByCourseIdAndStudentId(
        Guid studentId,
        Guid courseId);

    Task<AnnouncementDto> GetAnnouncementByStudentId(
        Guid studentId,
        Guid announcementId);

    // Student File Methods
    Task<List<StudyFileDto>> GetFilesByAnnouncementIdAndStudentId(
        Guid studentId,
        Guid announcementId);
}

public class AnnouncementService : IAnnouncementService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IStudyFileService _studyFileService;

    public AnnouncementService(StudyDbContext dbContext, IMapper mapper, IStudyFileService studyFileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _studyFileService = studyFileService;
    }

    // ----------------
    // CRUD METHODS
    // ----------------

    public async Task<List<AnnouncementDto>> GetAll()
    {
        var announcements = await _dbContext.Announcements
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto> GetById(Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto> Create(CreateAnnouncementDto dto)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == dto.CourseId);

        if (!courseExists)
            throw new KeyNotFoundException("Course not found");

        var announcement = _mapper.Map<Announcement>(dto);
        announcement.Id = Guid.NewGuid();
        announcement.CreatedAt = DateTime.UtcNow;

        _dbContext.Announcements.Add(announcement);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto> Update(Guid announcementId, UpdateAnnouncementDto dto)
    {
        var announcement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        announcement.Name = dto.Name ?? announcement.Name;
        announcement.Content = dto.Content ?? announcement.Content;
        announcement.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task Delete(Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        _dbContext.Announcements.Remove(announcement);
        await _dbContext.SaveChangesAsync();
    }

    // -----------------
    // ADMIN METHODS
    // -----------------

    public async Task<AnnouncementDto> AdminUpdateAnnouncement(Guid announcementId, AdminUpdateAnnouncementDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StudyFileDto>> GetFiles(Guid announcementId)
    {
        throw new NotImplementedException();
    }


    // -----------------
    // TEACHER METHODS
    // -----------------

    // Teacher CRUD Methods
    public async Task<List<AnnouncementDto>> GetAnnouncementsByTeacherId(
        Guid teacherId)
    {
        var announcements = await _dbContext.Announcements
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .Where(a => a.Course.TeacherId == teacherId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto> GetAnnouncementByTeacherId(Guid teacherId, Guid announcementId)
    {
        var announcement = await VerifyTeacherAnnouncement(teacherId, announcementId);

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto> CreateAnnouncementByTeacherId(Guid teacherId, CreateAnnouncementDto dto)
    {
        await VerifyTeacherCourse(teacherId, dto.CourseId);

        var exists = await _dbContext.Announcements
            .AnyAsync(a =>
                a.Name == dto.Name &&
                a.CourseId == dto.CourseId);

        if (exists)
            throw new ExceptionMiddleware.ConflictException(
                "Announcement with this Name already exists in this course");

        var announcement = _mapper.Map<Announcement>(dto);

        _dbContext.Announcements.Add(announcement);

        await _dbContext.SaveChangesAsync();

        announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .FirstAsync(a => a.Id == announcement.Id);

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto> UpdateAnnouncementByTeacherId(Guid teacherId, Guid announcementId,
        UpdateAnnouncementDto dto)
    {
        var announcement = await VerifyTeacherAnnouncement(teacherId, announcementId);

        announcement.Name = dto.Name ?? announcement.Name;
        announcement.Content = dto.Content ?? announcement.Content;
        announcement.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task DeleteAnnouncementByTeacherId(Guid teacherId, Guid announcementId)
    {
        var announcement = await VerifyTeacherAnnouncement(teacherId, announcementId);
        _dbContext.Announcements.Remove(announcement);
        await _dbContext.SaveChangesAsync();
    }

    // Teacher Methods
    public async Task<List<AnnouncementDto>> GetAnnouncementsByCourseIdAndTeacherId(Guid teacherId, Guid courseId)
    {
        await VerifyTeacherCourse(teacherId, courseId);

        var announcements = await _dbContext.Announcements
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .Where(a => a.CourseId == courseId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    // Teacher File Methods
    public async Task<StudyFileDto> UploadFileToAnnouncementByTeacherId(Guid teacherId, Guid announcementId,
        IFormFile file, string? altText = null)
    {
        await VerifyTeacherAnnouncement(teacherId, announcementId);

        var uploadedFile = await _studyFileService.UploadFile(file, teacherId, altText);

        await _studyFileService.AttachToAnnouncement(uploadedFile.Id, announcementId);

        return _mapper.Map<StudyFileDto>(uploadedFile);
    }

    public async Task<List<StudyFileDto>> GetFilesByAnnouncementIdAndTeacherId(Guid teacherId, Guid announcementId)
    {
        await VerifyTeacherAnnouncement(teacherId, announcementId);

        return await _studyFileService.GetFilesByAnnouncementId(announcementId);
    }

    public async Task AttachFileToAnnouncementByTeacherId(Guid teacherId, Guid announcementId, Guid fileId)
    {
        await VerifyTeacherAnnouncement(teacherId, announcementId);

        await _studyFileService.AttachToAnnouncement(fileId, announcementId);
    }

    public async Task RemoveFileFromAnnouncementByTeacherId(Guid teacherId, Guid announcementId, Guid fileId)
    {
        await VerifyTeacherAnnouncement(teacherId, announcementId);

        await _studyFileService.RemoveFromAnnouncement(fileId, announcementId);
    }

    // -----------------
    // STUDENT METHODS
    // -----------------
    public async Task<List<AnnouncementDto>> GetAnnouncementsByStudentId(Guid studentId)
    {
        var announcements = await _dbContext.Announcements
            .Include(a => a.StudyFiles)
            .Include(a => a.Course)
            .ThenInclude(c => c.CourseStudents)
            .Where(a => a.Course.CourseStudents.Any(cs => cs.StudentId == studentId))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    public async Task<List<AnnouncementDto>> GetAnnouncementsByCourseIdAndStudentId(Guid studentId, Guid courseId)
    {
        await VerifyStudentCourse(studentId, courseId);

        var announcements = await _dbContext.Announcements
            .Include(a => a.StudyFiles)
            .Include(a => a.Course)
            .Where(a => a.CourseId == courseId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto> GetAnnouncementByStudentId(Guid studentId, Guid announcementId)
    {
        var announcement = await VerifyStudentAnnouncement(studentId, announcementId);

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    // Student File Methods
    public async Task<List<StudyFileDto>> GetFilesByAnnouncementIdAndStudentId(Guid studentId, Guid announcementId)
    {
        await VerifyStudentAnnouncement(studentId, announcementId);

        return await _studyFileService.GetFilesByAnnouncementId(announcementId);
    }


    // -----------------
    // HELPER METHODS
    // -----------------
    private async Task<Course> VerifyTeacherCourse(
        Guid teacherId,
        Guid courseId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException(
                $"Course with id {courseId} not found.");

        if (course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this course.");

        return course;
    }

    private async Task<Announcement> VerifyTeacherAnnouncement(
        Guid teacherId,
        Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException(
                $"Announcement with id {announcementId} not found.");

        if (announcement.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this announcement.");

        return announcement;
    }

    private async Task<Course> VerifyStudentCourse(
        Guid studentId,
        Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException(
                $"Course with id {courseId} not found.");

        var isEnrolled = course.CourseStudents
            .Any(cs => cs.StudentId == studentId);

        if (!isEnrolled)
            throw new UnauthorizedAccessException(
                "Student does not have access to this course.");

        return course;
    }

    private async Task<Announcement> VerifyStudentAnnouncement(
        Guid studentId,
        Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .ThenInclude(c => c.CourseStudents)
            .Include(a => a.StudyFiles)
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException(
                $"Announcement with id {announcementId} not found.");

        var isEnrolled = announcement.Course.CourseStudents
            .Any(cs => cs.StudentId == studentId);

        if (!isEnrolled)
            throw new UnauthorizedAccessException(
                "Student does not have access to this announcement.");

        return announcement;
    }
}