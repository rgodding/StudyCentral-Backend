using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IStudyFileService
{
    Task<StudyFile> UploadFile(IFormFile file, Guid userId, FileType type, string? altText = null);

    Task DeleteFile(Guid fileId);

    Task<string> GetFileUrl(Guid fileId);
}

public class StudyFileService : IStudyFileService
{
    private readonly StudyDbContext _dbContext;
    private readonly IBlobService _blobService;

    public StudyFileService(StudyDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async Task<StudyFile> UploadFile(
        IFormFile file,
        Guid userId,
        FileType type,
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
            FileType = type,
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
}