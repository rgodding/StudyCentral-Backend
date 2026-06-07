using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.Course;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IStudyFolderService
{
    // CRUD
    Task<List<StudyFolderDto>> GetAll();
    Task<StudyFolderDto> GetById(Guid id);
    Task<StudyFolderDto> Create(CreateStudyFolderDto dto);
    Task<StudyFolderDto> Update(Guid id, UpdateStudyFolderDto dto);
    Task Delete(Guid id);
    
    // Admin Methods
    Task<List<StudyFileDto>> GetFiles(Guid folderId);

    // Teacher Folder Methods
    Task<List<StudyFolderDto>> GetFoldersByCourseIdAndTeacherId(Guid teacherId, Guid courseId,
        Guid? parentFolderId = null);

    Task<StudyFolderDto> GetFolderByTeacherId(Guid teacherId, Guid folderId);
    Task<StudyFolderDto> CreateFolderByTeacherId(Guid teacherId, CreateStudyFolderDto dto);
    Task<StudyFolderDto> UpdateFolderByTeacherId(Guid teacherId, Guid folderId, UpdateStudyFolderDto dto);
    Task DeleteFolderByTeacherId(Guid teacherId, Guid folderId);
    Task<StudyFolderDto> MoveFolderByTeacherId(Guid teacherId, Guid folderId, Guid? newParentFolderId);

    // Teacher File Methods
    Task<List<StudyFileDto>> GetFilesByFolderIdAndTeacherId(Guid teacherId, Guid folderId);
    Task<StudyFileDto> GetFileByIdAndTeacherId(Guid teacherId, Guid fileId);
    Task<StudyFileDto> CreateFileByTeacherId(Guid teacherId, Guid folderId, UploadFileToFolderRequest request);
    Task RemoveFileFromFolderByTeacherId(Guid teacherId, Guid folderId, Guid fileId);

    // Student Folder Methods
    Task<List<StudyFolderDto>> GetFoldersByCourseIdAndStudentId(Guid studentId, Guid courseId,
        Guid? parentFolderId = null);

    Task<StudyFolderDto> GetFolderByStudentId(Guid studentId, Guid folderId);
    Task<List<StudyFileDto>> GetFilesByFolderIdAndStudentId(Guid studentId, Guid folderId);
    Task<StudyFileDto> GetFileByIdAndStudentId(Guid studentId, Guid fileId);
}

public class StudyFolderService : IStudyFolderService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IStudyFileService _studyFileService;

    public StudyFolderService(StudyDbContext dbContext, IMapper mapper, IStudyFileService studyFileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _studyFileService = studyFileService;
    }

    // --------------
    //  CRUD METHODS
    // --------------

    public async Task<List<StudyFolderDto>> GetAll()
    {
        var folders = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .ToListAsync();

        return _mapper.Map<List<StudyFolderDto>>(folders);
    }

    public async Task<StudyFolderDto> GetById(Guid id)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null)
            throw new KeyNotFoundException($"StudyFolder with id '{id}' not found.");

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<StudyFolderDto> Create(CreateStudyFolderDto dto)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == dto.CourseId);

        if (!courseExists)
            throw new KeyNotFoundException($"Course with id '{dto.CourseId}' not found.");

        if (dto.ParentFolderId.HasValue)
        {
            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f => f.Id == dto.ParentFolderId.Value);

            if (parentFolder == null)
                throw new KeyNotFoundException($"Parent folder with id '{dto.ParentFolderId.Value}' not found.");

            if (parentFolder.CourseId != dto.CourseId)
                throw new InvalidOperationException(
                    "Parent folder must belong to the same course.");
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
            throw new KeyNotFoundException($"StudyFolder with id '{id}' not found.");

        if (dto.ParentFolderId.HasValue)
        {
            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f => f.Id == dto.ParentFolderId.Value);

            if (parentFolder == null)
                throw new KeyNotFoundException($"Parent folder with id '{dto.ParentFolderId}' not found.");

            if (parentFolder.CourseId != folder.CourseId)
                throw new InvalidOperationException("Parent folder must belong to the same course.");

            await ValidateFolderMove(id, dto.ParentFolderId);
        }

        folder.Name = dto.Name ?? folder.Name;
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
            throw new KeyNotFoundException($"StudyFolder with id '{id}' not found.");

        var hasChildren = await _dbContext.StudyFolders
            .AnyAsync(f => f.ParentFolderId == id);

        if (hasChildren)
            throw new InvalidOperationException("Cannot delete a folder that has child folders.");

        var hasFiles = await _dbContext.StudyFiles
            .AnyAsync(f => f.StudyFolderId == id);

        if (hasFiles)
            throw new InvalidOperationException("Cannot delete a folder that has files.");

        _dbContext.StudyFolders.Remove(folder);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<StudyFileDto>> GetFiles(Guid folderId)
    {
        var folderExists = await _dbContext.StudyFolders
            .AnyAsync(f => f.Id == folderId);

        if (!folderExists)
            throw new KeyNotFoundException($"StudyFolder with id '{folderId}' not found.");

        var files = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .Where(f => f.StudyFolderId == folderId)
            .OrderBy(f => f.FileName)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    // -----------------
    // Teacher Folder Methods
    // -----------------

    public async Task<List<StudyFolderDto>> GetFoldersByCourseIdAndTeacherId(Guid teacherId, Guid courseId,
        Guid? parentFolderId = null)
    {
        await VerifyTeacherCourse(teacherId, courseId);

        var folders = await _dbContext.StudyFolders
            .Include(f => f.ChildFolders)
            .Where(f =>
                f.CourseId == courseId &&
                f.ParentFolderId == parentFolderId)
            .OrderBy(f => f.Name)
            .ToListAsync();

        return _mapper.Map<List<StudyFolderDto>>(folders);
    }

    public async Task<StudyFolderDto> GetFolderByTeacherId(
        Guid teacherId,
        Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException(
                $"StudyFolder with id {folderId} not found.");

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
                    f.Id == dto.ParentFolderId);

            if (parentFolder == null)
                throw new KeyNotFoundException(
                    $"Parent folder with id {dto.ParentFolderId} not found.");

            if (parentFolder.CourseId != dto.CourseId)
                throw new InvalidOperationException(
                    "Parent folder must belong to the same course.");
        }

        var folder = _mapper.Map<StudyFolder>(dto);

        _dbContext.StudyFolders.Add(folder);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<StudyFolderDto> UpdateFolderByTeacherId(Guid teacherId, Guid folderId, UpdateStudyFolderDto dto)
    {
        var folder = await VerifyTeacherFolder(teacherId, folderId);

        if (dto.ParentFolderId.HasValue)
        {
            var parentFolder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f =>
                    f.Id == dto.ParentFolderId);

            if (parentFolder == null)
                throw new KeyNotFoundException(
                    $"Parent folder with id {dto.ParentFolderId} not found.");

            if (parentFolder.CourseId != folder.CourseId)
                throw new InvalidOperationException(
                    "Parent folder must belong to the same course.");

            if (parentFolder.Id == folder.Id)
                throw new InvalidOperationException(
                    "A folder cannot be its own parent.");

            await ValidateFolderMove(folderId, dto.ParentFolderId);
        }

        folder.Name = dto.Name ?? folder.Name;
        folder.ParentFolderId = dto.ParentFolderId;
        folder.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task DeleteFolderByTeacherId(Guid teacherId, Guid folderId)
    {
        var folder = await VerifyTeacherFolder(teacherId, folderId);

        var hasChildren = await _dbContext.StudyFolders
            .AnyAsync(f => f.ParentFolderId == folderId);

        if (hasChildren)
            throw new InvalidOperationException(
                "Cannot delete a folder that has child folders.");

        var hasFiles = await _dbContext.StudyFiles
            .AnyAsync(f => f.StudyFolderId == folderId);

        if (hasFiles)
            throw new InvalidOperationException(
                "Cannot delete a folder that has files.");

        _dbContext.StudyFolders.Remove(folder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<StudyFolderDto> MoveFolderByTeacherId(Guid teacherId, Guid folderId, Guid? newParentFolderId)
    {
        var folder = await VerifyTeacherFolder(teacherId, folderId);

        if (newParentFolderId.HasValue)
        {
            var newParent = await VerifyTeacherFolder(teacherId, newParentFolderId.Value);

            if (newParent.CourseId != folder.CourseId)
                throw new InvalidOperationException(
                    "New parent folder must belong to the same course.");

            if (newParent.Id == folder.Id)
                throw new InvalidOperationException(
                    "A folder cannot be its own parent.");

            await ValidateFolderMove(folderId, newParentFolderId);
        }

        folder.ParentFolderId = newParentFolderId;
        folder.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<List<StudyFileDto>> GetFilesByFolderIdAndTeacherId(Guid teacherId, Guid folderId)
    {
        await VerifyTeacherFolder(teacherId, folderId);

        var files = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .Where(f => f.StudyFolderId == folderId)
            .OrderBy(f => f.FileName)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    public async Task<StudyFileDto> GetFileByIdAndTeacherId(Guid teacherId, Guid fileId)
    {
        var file = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"StudyFile with id '{fileId}' not found.");

        if (file.StudyFolderId == null)
            throw new InvalidOperationException("File is not associated with any folder.");

        await VerifyTeacherFolder(teacherId, file.StudyFolderId.Value);

        return _mapper.Map<StudyFileDto>(file);
    }

    public async Task<StudyFileDto> CreateFileByTeacherId(Guid teacherId, Guid folderId,
        UploadFileToFolderRequest request)
    {
        await VerifyTeacherFolder(teacherId, folderId);

        var studyFile = await _studyFileService.UploadFile(
            request.File,
            teacherId,
            request.AltText);

        await _studyFileService.AttachToFolder(studyFile.Id, folderId);

        var file = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .FirstAsync(f => f.Id == studyFile.Id);

        return _mapper.Map<StudyFileDto>(file);
    }

    public async Task RemoveFileFromFolderByTeacherId(Guid teacherId, Guid folderId, Guid fileId)
    {
        await VerifyTeacherFolder(teacherId, folderId);

        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"StudyFile with id '{fileId}' not found.");

        if (file.StudyFolderId != folderId)
            throw new InvalidOperationException("File is not in the specified folder.");

        await _studyFileService.RemoveFromFolder(fileId);
    }

    // -----------------
    // Student Folder Methods
    // -----------------

    public async Task<List<StudyFolderDto>> GetFoldersByCourseIdAndStudentId(Guid studentId, Guid courseId,
        Guid? parentFolderId = null)
    {
        await VerifyStudentCourse(studentId, courseId);

        var folders = await _dbContext.StudyFolders
            .Include(f => f.ChildFolders)
            .Where(f =>
                f.CourseId == courseId &&
                f.ParentFolderId == parentFolderId)
            .OrderBy(f => f.Name)
            .ToListAsync();

        return _mapper.Map<List<StudyFolderDto>>(folders);
    }

    public async Task<StudyFolderDto> GetFolderByStudentId(
        Guid studentId,
        Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .Include(f => f.ChildFolders)
            .Include(f => f.StudyFiles)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException(
                $"StudyFolder with id {folderId} not found.");

        var isEnrolled = await _dbContext.CourseStudents
            .AnyAsync(e =>
                e.CourseId == folder.CourseId &&
                e.StudentId == studentId);

        if (!isEnrolled)
            throw new UnauthorizedAccessException(
                "Student does not have access to this folder.");

        return _mapper.Map<StudyFolderDto>(folder);
    }

    public async Task<List<StudyFileDto>> GetFilesByFolderIdAndStudentId(Guid studentId, Guid folderId)
    {
        await VerifyStudentFolder(studentId, folderId);

        var files = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .Where(f => f.StudyFolderId == folderId)
            .OrderBy(f => f.FileName)
            .ToListAsync();

        return _mapper.Map<List<StudyFileDto>>(files);
    }

    public async Task<StudyFileDto> GetFileByIdAndStudentId(Guid studentId, Guid fileId)
    {
        var file = await _dbContext.StudyFiles
            .Include(f => f.UploadedBy)
            .FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            throw new KeyNotFoundException($"StudyFile with id '{fileId}' not found.");

        if (file.StudyFolderId == null)
            throw new InvalidOperationException("File is not associated with any folder.");

        await VerifyStudentFolder(studentId, file.StudyFolderId.Value);

        return _mapper.Map<StudyFileDto>(file);
    }

    // -----------------
    // HELPER METHODS
    // ----------------

    // Teacher Verifications
    private async Task<StudyFolder> VerifyTeacherFolder(
        Guid teacherId,
        Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException(
                $"StudyFolder with id {folderId} not found.");

        if (folder.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this folder");

        return folder;
    }

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
                "Teacher does not have access to this course");

        return course;
    }

    // Student Verifications
    private async Task<StudyFolder> VerifyStudentFolder(
        Guid studentId,
        Guid folderId)
    {
        var folder = await _dbContext.StudyFolders
            .Include(f => f.Course)
            .ThenInclude(c => c.CourseStudents)
            .FirstOrDefaultAsync(f => f.Id == folderId);

        if (folder == null)
            throw new KeyNotFoundException(
                $"StudyFolder with id {folderId} not found.");

        var isEnrolled = folder.Course.CourseStudents
            .Any(cs => cs.StudentId == studentId);

        if (!isEnrolled)
            throw new UnauthorizedAccessException(
                "Student does not have access to this folder");

        return folder;
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
                "Student is not enrolled in this course");

        return course;
    }

    // General Helpers
    private async Task ValidateFolderMove(
        Guid folderId,
        Guid? newParentFolderId)
    {
        if (!newParentFolderId.HasValue)
            return;

        Guid? currentParentId = newParentFolderId.Value;

        while (currentParentId.HasValue)
        {
            if (currentParentId.Value == folderId)
                throw new InvalidOperationException(
                    "Cannot move a folder into itself or one of its descendants.");

            var parentId = currentParentId.Value;

            currentParentId = await _dbContext.StudyFolders
                .Where(f => f.Id == parentId)
                .Select(f => f.ParentFolderId)
                .FirstOrDefaultAsync();
        }
    }
}