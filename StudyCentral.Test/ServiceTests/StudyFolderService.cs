using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.Test.ServiceTests;

public interface IStudyFolderService
{
    // CRUD
    Task<List<StudyFolderDto>> GetAll();
    Task<StudyFolderDto> GetById(Guid id);
    Task<StudyFolderDto> Create(CreateStudyFolderDto dto);
    Task<StudyFolderDto> Update(Guid id, UpdateStudyFolderDto dto);
    Task Delete(Guid id);

    // Teacher Methods
    Task<List<StudyFolderDto>> GetFoldersByCourseIdAndTeacherId(Guid teacherId, Guid courseId);
    Task<StudyFolderDto> GetFolderByTeacherId(Guid teacherId, Guid folderId);
    Task<StudyFolderDto> CreateFolderByTeacherId(Guid teacherId, CreateStudyFolderDto dto);

    Task<StudyFolderDto> UpdateFolderByTeacherId(Guid teacherId, Guid folderId, UpdateStudyFolderDto dto);

    Task DeleteFolderByTeacherId(Guid teacherId, Guid folderId);

    // Student Methods
    Task<List<StudyFolderDto>> GetFoldersByCourseIdAndStudentId(Guid studentId, Guid courseId);
    Task<StudyFolderDto> GetFolderByStudentId(Guid studentId, Guid folderId);
}

public class StudyFolderService : IStudyFolderService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public StudyFolderService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // -----------------
    // CRUD
    // -----------------

    public async Task<List<StudyFolderDto>> GetAll()
    {
        var folders = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .ToListAsync();

        return _mapper.Map<List<StudyFolderDto>>(folders);
    }

    public async Task<StudyFolderDto> GetById(Guid id)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id {id} not found.");

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<StudyFolderDto> Create(CreateStudyFolderDto dto)
    {
        var course = await _dbContext.Courses.FindAsync(dto.CourseId);

        if (course == null)
            throw new KeyNotFoundException($"Course with id {dto.CourseId} not found.");

        if (dto.ParentFolderId.HasValue)
        {
            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f =>
                    f.Id == dto.ParentFolderId &&
                    f.CourseId == dto.CourseId);

            if (parentFolder == null)
                throw new KeyNotFoundException(
                    "Parent folder not found");
        }

        var folder = _mapper.Map<StudyFolder>(dto);

        _dbContext.StudyFolders.Add(folder);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<StudyFolderDto> Update(Guid id, UpdateStudyFolderDto dto)
    {
        var folder = await _dbContext.StudyFolders
            .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id {id} not found.");

        if (dto.ParentFolderId.HasValue)
        {
            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f =>
                    f.Id == dto.ParentFolderId &&
                    f.CourseId == folder.CourseId);

            if (parentFolder == null)
                throw new KeyNotFoundException(
                    "Parent folder not found");
        }

        folder.Name = dto.Name;
        folder.ParentFolderId = dto.ParentFolderId;
        folder.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task Delete(Guid id)
    {
        var folder = await _dbContext.StudyFolders
            .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id {id} not found.");

        _dbContext.StudyFolders.Remove(folder);
        await _dbContext.SaveChangesAsync();
    }

    // -----------------
    // Teacher Methods
    // -----------------
    public async Task<List<StudyFolderDto>> GetFoldersByCourseIdAndTeacherId(Guid teacherId, Guid courseId)
    {
        await VerifyTeacherCourse(teacherId, courseId);

        var folders = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .Where(f => f.CourseId == courseId)
            .ToListAsync();

        return _mapper.Map<List<StudyFolderDto>>(folders);
    }

    public async Task<StudyFolderDto> GetFolderByTeacherId(Guid teacherId, Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id {folderId} not found.");

        if (folder.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this folder");

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<StudyFolderDto> CreateFolderByTeacherId(Guid teacherId, CreateStudyFolderDto dto)
    {
        await VerifyTeacherCourse(teacherId, dto.CourseId);
        if (dto.ParentFolderId.HasValue)
        {
            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f =>
                    f.Id == dto.ParentFolderId &&
                    f.CourseId == dto.CourseId);

            if (parentFolder == null)
                throw new KeyNotFoundException(
                    "Parent folder not found");
        }

        var folderExists = await _dbContext.StudyFolders
            .AnyAsync(f =>
                f.CourseId == dto.CourseId &&
                f.ParentFolderId == dto.ParentFolderId &&
                f.Name == dto.Name);

        if (folderExists)
            throw new InvalidOperationException(
                "A folder with the same name already exists in this location");

        var folder = _mapper.Map<StudyFolder>(dto);
        _dbContext.StudyFolders.Add(folder);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<StudyFolderDto> UpdateFolderByTeacherId(Guid teacherId, Guid folderId, UpdateStudyFolderDto dto)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id {folderId} not found.");

        if (folder.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this folder");

        var duplicateFolder = await _dbContext.StudyFolders
            .AnyAsync(f =>
                f.Id != folderId &&
                f.CourseId == folder.CourseId &&
                f.ParentFolderId == dto.ParentFolderId &&
                f.Name == dto.Name);
        
        if (duplicateFolder)
            throw new InvalidOperationException(
                "A folder with the same name already exists in this location");

        if (dto.ParentFolderId.HasValue)
        {
            if(dto.ParentFolderId == folder.Id)
                throw new InvalidOperationException(
                    "A folder cannot be its own parent");

            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f =>
                    f.Id == dto.ParentFolderId);
            
            if (parentFolder == null)
                throw new KeyNotFoundException(
                    "Parent folder not found");
            
            if (parentFolder.CourseId != folder.CourseId)
                throw new InvalidOperationException(
                    "Parent folder must be in the same course");
        }
        
        folder.Name = dto.Name ?? folder.Name;
        folder.ParentFolderId = dto.ParentFolderId;
        folder.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task DeleteFolderByTeacherId(Guid teacherId, Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .FirstOrDefaultAsync(f => f.Id == folderId);
        
        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id {folderId} not found.");
        
        if (folder.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this folder");
        
        if (folder.ChildFolders.Any())
            throw new InvalidOperationException(
                "Cannot delete a folder that contains subfolders");

        if (folder.StudyFiles.Any())
            throw new InvalidOperationException(
                "Cannot delete a folder that contains files");
        
        _dbContext.StudyFolders.Remove(folder);
        await _dbContext.SaveChangesAsync();
    }

    // -----------------
    // Student Methods
    // -----------------

    public async Task<List<StudyFolderDto>> GetFoldersByCourseIdAndStudentId(Guid studentId, Guid courseId)
    {
        await VerifyStudentCourse(studentId, courseId);

        var folders = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .Where(f => f.CourseId == courseId)
            .ToListAsync();

        return _mapper.Map<List<StudyFolderDto>>(folders);
    }

    public async Task<StudyFolderDto> GetFolderByStudentId(Guid studentId, Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException(
                $"StudyFolder with id {folderId} not found.");

        await VerifyStudentCourse(
            studentId,
            folder.CourseId);

        return _mapper.Map<StudyFolderDto>(folder);
    }


    private async Task VerifyTeacherCourse(
        Guid teacherId,
        Guid courseId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this course");
    }

    private async Task VerifyStudentCourse(
        Guid studentId,
        Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        var isEnrolled = course.CourseStudents
            .Any(cs => cs.StudentId == studentId);

        if (!isEnrolled)
            throw new UnauthorizedAccessException(
                "Student is not enrolled in this course");
    }
}