using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAnnouncementService
{
    Task<List<AnnouncementDto>> GetAll();
    Task<AnnouncementDto> GetById(Guid announcementId);
    Task<AnnouncementDto> Create(CreateAnnouncementDto dto);
    Task<AnnouncementDto> Update(Guid announcementId, UpdateAnnouncementDto dto);
    Task Delete(Guid announcementId);
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
    // GET ALL
    // ----------------
    public async Task<List<AnnouncementDto>> GetAll()
    {
        var announcements = await _dbContext.Announcements
            .Include(a => a.Course)
            .ToListAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }

    // ----------------
    // GET BY ID
    // ----------------
    public async Task<AnnouncementDto> GetById(Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        return _mapper.Map<AnnouncementDto>(announcement);
    }

    // ----------------
    // CREATE
    // ----------------
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

    // ----------------
    // UPDATE
    // ----------------
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

    // ----------------
    // DELETE
    // ----------------
    public async Task Delete(Guid announcementId)
    {
        var announcement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException("Announcement not found");

        _dbContext.Announcements.Remove(announcement);
        await _dbContext.SaveChangesAsync();
    }
}