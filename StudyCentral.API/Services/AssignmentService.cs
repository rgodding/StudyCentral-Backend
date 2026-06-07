using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Admin.Assignment;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAssignmentService
{
    // CRUD METHODS
    Task<List<AssignmentDto>> GetAll();
    Task<AssignmentDto> GetById(Guid assignmentId);
    Task<AssignmentDto> Create(CreateAssignmentDto dto);
    Task<AssignmentDto> Update(Guid assignmentId, UpdateAssignmentDto dto);
    Task Delete(Guid assignmentId);
    
    // Admin Methods
    Task<AssignmentDto> AdminUpdateAssignment(Guid assignmentId, AdminUpdateAssignmentDto dto);
    Task<List<StudyFileDto>> GetFiles(Guid assignmentId);

    // Teacher CRUD Methods
    Task<List<AssignmentDto>> GetAssignmentsByTeacherId(
        Guid teacherId);

    Task<AssignmentDto> GetAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId);

    Task<AssignmentDto> CreateAssignmentByTeacherId(
        Guid teacherId,
        CreateAssignmentDto dto);

    Task<AssignmentDto> UpdateAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId,
        UpdateAssignmentDto dto);

    Task DeleteAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId);

    // Teacher Methods
    Task<List<AssignmentDto>> GetAssignmentsByCourseIdAndTeacherId(
        Guid teacherId,
        Guid courseId);

    // Teacher File Methods
    Task<List<StudyFileDto>> GetFilesByAssignmentIdAndTeacherId(
        Guid teacherId,
        Guid assignmentId);

    Task<StudyFileDto> UploadFileToAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId,
        IFormFile file);

    Task DeleteFileFromAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId,
        Guid fileId);

    Task AttachFileToAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId,
        Guid fileId);

    Task DetachFileFromAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId,
        Guid fileId);

    // Student Methods
    Task<List<AssignmentDto>> GetAssignmentsByStudentId(
        Guid studentId);

    Task<List<AssignmentDto>> GetAssignmentsByCourseIdAndStudentId(
        Guid studentId,
        Guid courseId);

    Task<AssignmentDto> GetAssignmentByStudentId(
        Guid studentId,
        Guid assignmentId);

    // Student File Methods
    Task<List<StudyFileDto>> GetFilesByAssignmentIdAndStudentId(
        Guid studentId,
        Guid assignmentId);
}

public class AssignmentService : IAssignmentService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IStudyFileService _fileService;

    public AssignmentService(StudyDbContext dbContext, IMapper mapper, IStudyFileService fileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileService = fileService;
    }

    // --------------
    //  CRUD METHODS
    // --------------

    public async Task<List<AssignmentDto>> GetAll()
    {
        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }

    public async Task<AssignmentDto> GetById(Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> Create(CreateAssignmentDto dto)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == dto.CourseId);

        if (!courseExists)
            throw new KeyNotFoundException("Course not found");

        var exists = await _dbContext.Assignments
            .AnyAsync(a => a.Name == dto.Name && a.CourseId == dto.CourseId);

        if (exists)
            throw new InvalidOperationException("Assignment with this name already exists in this course");

        var assignment = _mapper.Map<Assignment>(dto);

        _dbContext.Assignments.Add(assignment);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> Update(Guid assignmentId, UpdateAssignmentDto dto)
    {
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != assignment.Name)
        {
            var exists = await _dbContext.Assignments
                .AnyAsync(a =>
                    a.Name == dto.Name &&
                    a.CourseId == assignment.CourseId &&
                    a.Id != assignmentId);

            if (exists)
                throw new InvalidOperationException("Assignment with this Name already exists in this course");

            assignment.Name = dto.Name;
        }

        assignment.Description = dto.Description ?? assignment.Description;
        assignment.Deadline = dto.Deadline ?? assignment.Deadline;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task Delete(Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }

    // --------------
    //  ADMIN METHODS
    // --------------
    
    public async Task<AssignmentDto> AdminUpdateAssignment(Guid assignmentId, AdminUpdateAssignmentDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StudyFileDto>> GetFiles(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    // --------------
    //  TEACHER METHODS
    // --------------

    // Teacher CRUD Methods
    public async Task<List<AssignmentDto>> GetAssignmentsByTeacherId(Guid teacherId)
    {
        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .Where(a => a.Course.TeacherId == teacherId)
            .OrderBy(a => a.Deadline)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }

    public async Task<AssignmentDto> GetAssignmentByTeacherId(Guid teacherId, Guid assignmentId)
    {
        var assignment = await VerifyTeacherAssignment(teacherId, assignmentId);
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> CreateAssignmentByTeacherId(
        Guid teacherId,
        CreateAssignmentDto dto)
    {
        await VerifyTeacherCourse(
            teacherId,
            dto.CourseId);

        var exists = await _dbContext.Assignments
            .AnyAsync(a => a.Name == dto.Name && a.CourseId == dto.CourseId);

        if (exists)
            throw new InvalidOperationException("Assignment with this name already exists in this course");
        var assignment = _mapper.Map<Assignment>(dto);

        _dbContext.Assignments.Add(assignment);
        await _dbContext.SaveChangesAsync();

        assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .FirstAsync(a => a.Id == assignment.Id);

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> UpdateAssignmentByTeacherId(Guid teacherId, Guid assignmentId,
        UpdateAssignmentDto dto)
    {
        var assignment = await VerifyTeacherAssignment(teacherId, assignmentId);


        assignment.Name = dto.Name ?? assignment.Name;
        assignment.Description = dto.Description ?? assignment.Description;
        assignment.Deadline = dto.Deadline ?? assignment.Deadline;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task DeleteAssignmentByTeacherId(Guid teacherId, Guid assignmentId)
    {
        var assignment = await VerifyTeacherAssignment(teacherId, assignmentId);

        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }

    // Teacher Methods
    public async Task<List<AssignmentDto>> GetAssignmentsByCourseIdAndTeacherId(Guid teacherId, Guid courseId)
    {
        await VerifyTeacherCourse(teacherId, courseId);

        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .Where(a => a.CourseId == courseId)
            .OrderBy(a => a.Deadline)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }

    // Teacher File Methods
    public async Task<List<StudyFileDto>> GetFilesByAssignmentIdAndTeacherId(Guid teacherId, Guid assignmentId)
    {
        var assignment = await VerifyTeacherAssignment(teacherId, assignmentId);

        return _mapper.Map<List<StudyFileDto>>(assignment.StudyFiles);
    }

    public async Task<StudyFileDto> UploadFileToAssignmentByTeacherId(Guid teacherId, Guid assignmentId, IFormFile file)
    {
        await VerifyTeacherAssignment(teacherId, assignmentId);

        var studyFile = await _fileService.UploadFile(file, teacherId);

        await _fileService.AttachToAssignment(studyFile.Id, assignmentId);

        return _mapper.Map<StudyFileDto>(studyFile);
    }

    public async Task DeleteFileFromAssignmentByTeacherId(Guid teacherId, Guid assignmentId, Guid fileId)
    {
        await VerifyTeacherAssignment(teacherId, assignmentId);

        var file = await _dbContext.StudyFiles
            .FirstOrDefaultAsync(f =>
                f.Id == fileId &&
                f.AssignmentId == assignmentId);

        if (file == null)
            throw new KeyNotFoundException("File not found in this assignment");

        await _fileService.DeleteFile(fileId);
    }

    public async Task AttachFileToAssignmentByTeacherId(Guid teacherId, Guid assignmentId, Guid fileId)
    {
        await VerifyTeacherAssignment(teacherId, assignmentId);

        await _fileService.AttachToAssignment(fileId, assignmentId);
    }

    public async Task DetachFileFromAssignmentByTeacherId(Guid teacherId, Guid assignmentId, Guid fileId)
    {
        await VerifyTeacherAssignment(teacherId, assignmentId);

        await _fileService.RemoveFromAssignment(fileId, assignmentId);
    }

    // --------------
    //  STUDENT METHODS
    // --------------

    // Student Methods
    public async Task<List<AssignmentDto>> GetAssignmentsByStudentId(
        Guid studentId)
    {
        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .Where(a => a.Course.CourseStudents
                .Any(cs => cs.StudentId == studentId))
            .OrderBy(a => a.Deadline)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }

    public async Task<List<AssignmentDto>> GetAssignmentsByCourseIdAndStudentId(
        Guid studentId,
        Guid courseId)
    {
        await VerifyStudentCourse(
            studentId,
            courseId);

        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .Where(a => a.CourseId == courseId)
            .OrderBy(a => a.Deadline)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }

    public async Task<AssignmentDto> GetAssignmentByStudentId(Guid studentId, Guid assignmentId)
    {
        var assignment = await VerifyStudentAssignment(studentId, assignmentId);
        return _mapper.Map<AssignmentDto>(assignment);
    }

    // Student File Methods
    public async Task<List<StudyFileDto>> GetFilesByAssignmentIdAndStudentId(Guid studentId, Guid assignmentId)
    {
        var assignment = await VerifyStudentAssignment(studentId, assignmentId);

        return _mapper.Map<List<StudyFileDto>>(assignment.StudyFiles);
    }

    // -----------------
    // HELPER METHODS
    // -----------------
    private async Task VerifyTeacherCourse(
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
    }

    private async Task<Assignment> VerifyTeacherAssignment(
        Guid teacherId,
        Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException(
                $"Assignment with id {assignmentId} not found.");

        if (assignment.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this assignment.");

        return assignment;
    }

    private async Task VerifyStudentCourse(
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
    }

    private async Task<Assignment> VerifyStudentAssignment(
        Guid studentId,
        Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .ThenInclude(c => c.CourseStudents)
            .Include(a => a.StudyFiles)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException(
                $"Assignment with id {assignmentId} not found.");

        var isEnrolled = assignment.Course.CourseStudents
            .Any(cs => cs.StudentId == studentId);

        if (!isEnrolled)
            throw new UnauthorizedAccessException(
                "Student does not have access to this assignment.");

        return assignment;
    }
}