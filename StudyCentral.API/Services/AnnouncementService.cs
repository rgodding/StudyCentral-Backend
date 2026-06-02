using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Announcements;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;


public interface IAnnouncementService
{
    Task<Announcement> GetAnnouncementById(Guid announcementId);
    public Task<List<Announcement>> GetAnnouncementsByCourseId(Guid courseId);
    
    Task<Announcement> CreateAnnouncement(Guid teacherId, Guid courseId, CreateAnnouncementDto request);
    Task<Announcement> UpdateAnnouncement(Guid teacherId, Guid announcementId, UpdateAnnouncementDto request);
    Task DeleteAnnouncement(Guid teacherId, Guid announcementId);
}

public class AnnouncementService : IAnnouncementService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;


    public AnnouncementService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Announcement> GetAnnouncementById(Guid announcementId)
    {
        return await  _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId)
            ?? throw new KeyNotFoundException("Announcement not found");
    }

    public async Task<List<Announcement>> GetAnnouncementsByCourseId(Guid courseId)
    {
        return await _dbContext.Announcements
            .Where(a => a.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Announcement> CreateAnnouncement(Guid teacherId, Guid courseId, CreateAnnouncementDto request)
    {
        var teacher = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher)
            ?? throw new KeyNotFoundException("Teacher not found");
        
        var course = await  _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacher.Id)
            ?? throw new KeyNotFoundException("Course not found");
        
        var announcement = _mapper.Map<Announcement>(request);
        announcement.Course = course;
        await _dbContext.Announcements.AddAsync(announcement);
        await _dbContext.SaveChangesAsync();
        
        return announcement;
        
    }

    public async Task<Announcement> UpdateAnnouncement(Guid teacherId, Guid announcementId, UpdateAnnouncementDto request)
    {
        var teacher = await  _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher)
            ?? throw new KeyNotFoundException("Teacher not found");
        
        var announcement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId)
            ?? throw new KeyNotFoundException("Announcement not found");
        
        announcement.Title = request.Title ?? announcement.Title;
        announcement.Content = request.Content ?? announcement.Content;
        await _dbContext.SaveChangesAsync();
        return announcement;
    }

    public async Task DeleteAnnouncement(Guid teacherId, Guid announcementId)
    {
        var teacher = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher)
            ?? throw new KeyNotFoundException("Teacher not found");
        
        var announcement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId)
            ?? throw new KeyNotFoundException("Announcement not found");
        
        _dbContext.Announcements.Remove(announcement);
        await _dbContext.SaveChangesAsync();
    }
}