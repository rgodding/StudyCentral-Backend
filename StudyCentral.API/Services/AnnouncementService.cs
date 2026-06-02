using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;


public interface IAnnouncementService
{
    Task<Announcement> CreateAnnouncement(Guid teacherId, Guid courseId, string title, string content);
}

public class AnnouncementService
{
    
}