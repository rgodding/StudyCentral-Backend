using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Services;

public interface IStudyFileService
{
    // Blob Operations
    Task<BlobFileResult> DownloadFile(Guid userId, Guid fileId);
    Task<StudyFile> UploadFile(IFormFile file, Guid userId, string? altText = null);
    Task DeleteFile(Guid fileId);

    // Folder Operations
    Task<List<StudyFileDto>> GetFilesByFolderId(Guid folderId);
    Task AttachToFolder(Guid fileId, Guid folderId);
    Task RemoveFromFolder(Guid fileId);

    // Announcement
    Task<List<StudyFileDto>> GetFilesByAnnouncementId(Guid announcementId);
    Task AttachToAnnouncement(Guid fileId, Guid announcementId);
    Task RemoveFromAnnouncement(Guid fileId, Guid announcementId);

    // Assignment
    Task<List<StudyFileDto>> GetFilesByAssignmentId(Guid assignmentId);
    Task AttachToAssignment(Guid fileId, Guid assignmentId);
    Task RemoveFromAssignment(Guid fileId, Guid assignmentId);

    // Submission
    Task<List<StudyFileDto>> GetFilesBySubmissionId(Guid submissionId);
    Task AttachToSubmission(Guid fileId, Guid submissionId);
    Task RemoveFromSubmission(Guid fileId, Guid submissionId);
}

public class StudyFileService : IStudyFileService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IBlobService _blobService;

    public StudyFileService(StudyDbContext dbContext, IBlobService blobService, IMapper mapper)
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _mapper = mapper;
    }

    // -----------------
    // Blob Operations
    // -----------------

    public async Task<BlobFileResult> DownloadFile(Guid userId, Guid fileId)
    {
        var file = await VerifyUserAccessToFile(userId, fileId);
        return await _blobService.GetFile(file.BlobName);
    }

    public async Task<StudyFile> UploadFile(
        IFormFile file,
        Guid userId,
        string? altText = null)
    {
        var userExists = await _dbContext.Users
            .AnyAsync(u => u.Id == userId);

        if (!userExists)
            throw new KeyNotFoundException("User not found");

        if (file == null || file.Length == 0)
            throw new InvalidOperationException("Invalid file");

        BlobUploadResult uploadResult;

        try
        {
            uploadResult = await _blobService.UploadFile(
                file.FileName,
                file);
        }
        catch (Exception ex)
        {
            throw new ExceptionMiddleware.InternalException(
                "Failed to upload file to blob storage",
                ex);
        }

        var studyFile = new StudyFile
        {
            FileName = uploadResult.FileName,
            BlobName = uploadResult.BlobName,
            ContentType = uploadResult.ContentType,
            Size = file.Length,
            FileType = GetFileType(file),
            AltText = altText,
            UploadedById = userId
        };

        _dbContext.StudyFiles.Add(studyFile);
        await _dbContext.SaveChangesAsync();

        return studyFile;
    }

    public async Task DeleteFile(Guid fileId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException("File not found");

        try
        {
            await _blobService.DeleteFile(file.BlobName);
        }
        catch (Exception ex)
        {
            throw new ExceptionMiddleware.InternalException(
                "Failed to delete file from blob storage",
                ex);
        }

        _dbContext.StudyFiles.Remove(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> GetFileUrl(Guid fileId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException("File not found");

        try
        {
            return await _blobService.GetBlobUrl(file.BlobName);
        }
        catch (Exception ex)
        {
            throw new ExceptionMiddleware.InternalException(
                $"Failed to retrieve URL for file '{file.Id}'",
                ex);
        }
    }

    // -----------------
    // Folder Operations
    // -----------------
    public async Task<List<StudyFileDto>> GetFilesByFolderId(Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException(
                $"Folder with ID '{folderId}' not found");

        var files = await _dbContext.StudyFiles
            .Where(f => f.StudyFolderId == folderId)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    public async Task AttachToFolder(Guid fileId, Guid folderId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var folder = await _dbContext.StudyFolders
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException($"Folder with ID '{folderId}' not found");

        if (file.StudyFolderId == folderId)
            throw new InvalidOperationException("File is already in the target folder");

        file.StudyFolderId = folderId;

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFromFolder(Guid fileId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        if (file.StudyFolderId == null)
            throw new InvalidOperationException("File is not in any folder");

        file.StudyFolderId = null;

        await _dbContext.SaveChangesAsync();
    }

    // -----------------
    // Announcement Operations
    // -----------------
    public async Task<List<StudyFileDto>> GetFilesByAnnouncementId(Guid announcementId)
    {
        var files = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .Where(f => f.AnnouncementId == announcementId)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    public async Task AttachToAnnouncement(Guid fileId, Guid announcementId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var announcement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == announcementId);

        if (announcement == null)
            throw new KeyNotFoundException($"Announcement with ID '{announcementId}' not found");

        if (file.AnnouncementId == announcementId)
            throw new InvalidOperationException("File is already attached to the target announcement");

        file.AnnouncementId = announcementId;
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFromAnnouncement(Guid fileId, Guid announcementId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        if (file.AnnouncementId != announcementId)
            throw new InvalidOperationException("File is not attached to the specified announcement");

        file.AnnouncementId = null;
        await _dbContext.SaveChangesAsync();
    }

    // -----------------
    // Assignment Operations
    // -----------------

    public async Task<List<StudyFileDto>> GetFilesByAssignmentId(Guid assignmentId)
    {
        var assignmentExists = await _dbContext.Assignments
            .AnyAsync(a => a.Id == assignmentId);

        if (!assignmentExists)
            throw new KeyNotFoundException($"Assignment with ID '{assignmentId}' not found");

        var files = await _dbContext.StudyFiles
            .Where(f => f.AssignmentId == assignmentId)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    public async Task AttachToAssignment(Guid fileId, Guid assignmentId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var assignmentExists = await _dbContext.Assignments
            .AnyAsync(a => a.Id == assignmentId);

        if (!assignmentExists)
            throw new KeyNotFoundException($"Assignment with ID '{assignmentId}' not found");

        if (file.AssignmentId == assignmentId)
            throw new InvalidOperationException("File is already attached to the target assignment");

        file.AssignmentId = assignmentId;
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFromAssignment(Guid fileId, Guid assignmentId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var assignmentExists = await _dbContext.Assignments
            .AnyAsync(a => a.Id == assignmentId);

        if (!assignmentExists)
            throw new KeyNotFoundException($"Assignment with ID '{assignmentId}' not found");

        if (file.AssignmentId != assignmentId)
            throw new InvalidOperationException("File is not attached to the specified assignment");

        file.AssignmentId = null;
        await _dbContext.SaveChangesAsync();
    }

    // -----------------
    // Submission Operations
    // -----------------
    public async Task<List<StudyFileDto>> GetFilesBySubmissionId(
        Guid submissionId)
    {
        var submissionExists = await _dbContext.Submissions
            .AnyAsync(s => s.Id == submissionId);

        if (!submissionExists)
            throw new KeyNotFoundException($"Submission with ID '{submissionId}' not found");

        var files = await _dbContext.StudyFiles
            .Where(f => f.SubmissionId == submissionId)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    public async Task AttachToSubmission(
        Guid fileId,
        Guid submissionId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var submissionExists = await _dbContext.Submissions
            .AnyAsync(s => s.Id == submissionId);

        if (!submissionExists)
            throw new KeyNotFoundException($"Submission with ID '{submissionId}' not found");

        if (file.SubmissionId == submissionId)
            throw new InvalidOperationException("File is already attached to the target submission");

        file.SubmissionId = submissionId;

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFromSubmission(
        Guid fileId,
        Guid submissionId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var submissionExists = await _dbContext.Submissions
            .AnyAsync(s => s.Id == submissionId);

        if (!submissionExists)
            throw new KeyNotFoundException($"Submission with ID '{submissionId}' not found");

        if (file.SubmissionId != submissionId)
            throw new InvalidOperationException("File is not attached to the specified submission");

        file.SubmissionId = null;

        await _dbContext.SaveChangesAsync();
    }


    // -----------------
    // Helper Methods
    // -----------------

    private FileType GetFileType(IFormFile file)
    {
        var contentType = file.ContentType.ToLower();

        if (contentType.StartsWith("image/"))
            return FileType.Image;

        if (contentType.StartsWith("video/"))
            return FileType.Video;

        if (contentType.StartsWith("audio/"))
            return FileType.Audio;

        if (contentType == "application/pdf")
            return FileType.Pdf;

        if (contentType.StartsWith("text/") ||
            contentType.Contains("word") ||
            contentType.Contains("document") ||
            contentType.Contains("officedocument") ||
            contentType.Contains("excel") ||
            contentType.Contains("spreadsheet") ||
            contentType.Contains("powerpoint") ||
            contentType.Contains("presentation"))
            return FileType.Document;

        return FileType.Other;
    }

    private async Task<StudyFile> VerifyUserAccessToFile(Guid userId, Guid fileId)
    {
        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"File with ID '{fileId}' not found");

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException($"User with ID '{userId}' not found");

        return user.Role switch
        {
            UserRole.Student => await VerifyStudentAccessToFile(userId, file),
            UserRole.Teacher => await VerifyTeacherAccessToFile(userId, file),
            UserRole.Admin => file, // Admin has access to all files
            _ => throw new InvalidOperationException("Invalid user role")
        };
    }

    private async Task<StudyFile> VerifyStudentAccessToFile(Guid studentId, StudyFile file)
    {
        if (file.AssignmentId.HasValue)
        {
            var assignment = await _dbContext.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == file.AssignmentId.Value);

            if (assignment != null)
            {
                var isEnrolled = await _dbContext.CourseStudents
                    .AnyAsync(cs =>
                        cs.CourseId == assignment.CourseId &&
                        cs.StudentId == studentId);

                if (isEnrolled)
                    return file;
            }
        }

        if (file.AnnouncementId.HasValue)
        {
            var announcement = await _dbContext.Announcements
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == file.AnnouncementId.Value);

            if (announcement != null)
            {
                var isEnrolled = await _dbContext.CourseStudents
                    .AnyAsync(cs =>
                        cs.CourseId == announcement.CourseId &&
                        cs.StudentId == studentId);

                if (isEnrolled)
                    return file;
            }
        }

        if (file.StudyFolderId.HasValue)
        {
            var folder = await _dbContext.StudyFolders
                .Include(f => f.Course)
                .FirstOrDefaultAsync(f => f.Id == file.StudyFolderId.Value);

            if (folder != null)
            {
                var isEnrolled = await _dbContext.CourseStudents
                    .AnyAsync(cs =>
                        cs.CourseId == folder.CourseId &&
                        cs.StudentId == studentId);

                if (isEnrolled)
                    return file;
            }
        }

        if (file.SubmissionId.HasValue)
        {
            var submission = await _dbContext.Submissions
                .Include(s => s.Assignment)
                .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(s => s.Id == file.SubmissionId.Value);

            if (submission != null &&
                submission.StudentId == studentId)
            {
                return file;
            }
        }

        throw new UnauthorizedAccessException(
            "Student does not have access to this file");
    }

    private async Task<StudyFile> VerifyTeacherAccessToFile(Guid teacherId, StudyFile file)
    {
        if (file.AssignmentId.HasValue)
        {
            var assignment = await _dbContext.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == file.AssignmentId.Value);

            if (assignment != null &&
                assignment.Course.TeacherId == teacherId)
            {
                return file;
            }
        }

        if (file.AnnouncementId.HasValue)
        {
            var announcement = await _dbContext.Announcements
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.Id == file.AnnouncementId.Value);

            if (announcement != null &&
                announcement.Course.TeacherId == teacherId)
            {
                return file;
            }
        }

        if (file.StudyFolderId.HasValue)
        {
            var folder = await _dbContext.StudyFolders
                .Include(f => f.Course)
                .FirstOrDefaultAsync(f => f.Id == file.StudyFolderId.Value);

            if (folder != null &&
                folder.Course.TeacherId == teacherId)
            {
                return file;
            }
        }

        if (file.SubmissionId.HasValue)
        {
            var submission = await _dbContext.Submissions
                .Include(s => s.Assignment)
                .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(s => s.Id == file.SubmissionId.Value);

            if (submission != null &&
                submission.Assignment.Course.TeacherId == teacherId)
            {
                return file;
            }
        }

        throw new UnauthorizedAccessException(
            "Teacher does not have access to this file");
    }
}