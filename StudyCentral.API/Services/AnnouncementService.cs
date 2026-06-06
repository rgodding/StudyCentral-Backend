using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Announcement;
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
    
    // Teacher Methods
    Task<List<AnnouncementDto>> GetAnnouncementsByTeacherId(
        Guid teacherId);

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

    // ----------------
    // CRUD METHODS
    // ----------------
    public async Task<List<AnnouncementDto>> GetAll()
    {
        var announcements = await _dbContext.Announcements
            .Include(a => a.Course)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto> GetById(Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
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
    // TEACHER METHODS
    // -----------------

    public async Task<List<AnnouncementDto>> GetAnnouncementsByTeacherId(Guid teacherId)
    {
        var announcements = await _dbContext.Announcements
            .Include(a => a.Course)
            .Where(a => a.Course.TeacherId == teacherId)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto> GetAnnouncementByTeacherId(Guid teacherId, Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a =>
                a.Id == announcementId &&
                a.Course.TeacherId == teacherId);
        
        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto> CreateAnnouncementByTeacherId(Guid teacherId, CreateAnnouncementDto dto)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == dto.CourseId && c.TeacherId == teacherId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        var announcement = _mapper.Map<Announcement>(dto);

        _dbContext.Announcements.Add(announcement);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto> UpdateAnnouncementByTeacherId(Guid teacherId, Guid announcementId, UpdateAnnouncementDto dto)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a =>
                a.Id == announcementId &&
                a.Course.TeacherId == teacherId);

        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        announcement.Name = dto.Name ?? announcement.Name;
        announcement.Content = dto.Content ?? announcement.Content;
        announcement.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task DeleteAnnouncementByTeacherId(Guid teacherId, Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a =>
                a.Id == announcementId &&
                a.Course.TeacherId == teacherId);
        
        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");
        
        _dbContext.Announcements.Remove(announcement);
        await _dbContext.SaveChangesAsync();       
    }
}