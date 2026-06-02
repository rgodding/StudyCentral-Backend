using StudyCentral.API.Models.Dtos.Announcements;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;


public interface IAnnouncementService
{
    Task<Announcement> GetAnnouncementById(Guid announcementId);
    public Task<List<Announcement>> GetAnnouncementsByCourse(Guid courseId);
    
    Task<Announcement> CreateAnnouncement(Guid teacherId, Guid courseId, CreateAnnouncementDto request);
    Task<Announcement> UpdateAnnouncement(Guid teacherId, Guid announcementId, UpdateAnnouncementDto request);
}

public class AnnouncementService : IAnnouncementService
{
    public Task<Announcement> GetAnnouncementById(Guid announcementId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Announcement>> GetAnnouncementsByCourse(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<Announcement> CreateAnnouncement(Guid teacherId, Guid courseId, CreateAnnouncementDto request)
    {
        throw new NotImplementedException();
    }

    public Task<Announcement> UpdateAnnouncement(Guid teacherId, Guid announcementId, UpdateAnnouncementDto request)
    {
        throw new NotImplementedException();
    }
}