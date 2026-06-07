using System.Security;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.Submission;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ISubmissionService
{
    // CRUD
    Task<List<SubmissionDto>> GetAll();
    Task<SubmissionDto> GetById(Guid submissionId);
    Task<SubmissionDto> Create(CreateSubmissionDto dto);
    Task<SubmissionDto> Update(Guid submissionId, UpdateSubmissionDto dto);
    Task Delete(Guid submissionId);

    // Teacher Methods
    Task<List<SubmissionDto>> GetSubmissionsByAssignmentIdAndTeacherId(Guid teacherId, Guid assignmentId);
    Task<SubmissionDto> GetSubmissionByTeacherId(Guid teacherId, Guid submissionId);
    Task<SubmissionDto> GradeSubmissionByTeacherId(Guid teacherId, Guid submissionId, GradeSubmissionRequest request);
    Task<List<StudyFileDto>> GetSubmissionFilesByTeacherId(Guid teacherId, Guid submissionId);

    // Student Methods
    Task<List<SubmissionDto>> GetSubmissionsByStudentId(
        Guid studentId);

    Task<SubmissionDto> GetSubmissionByStudentId(
        Guid studentId,
        Guid submissionId);

    Task<SubmissionDto> CreateSubmissionByStudentId(
        Guid studentId,
        CreateSubmissionDto dto);

    Task<SubmissionDto> UpdateSubmissionByStudentId(
        Guid studentId,
        Guid submissionId,
        UpdateSubmissionDto dto);

    Task DeleteSubmissionByStudentId(
        Guid studentId,
        Guid submissionId);

    // Student File Methods
    Task<List<StudyFileDto>> GetSubmissionFilesByStudentId(
        Guid studentId,
        Guid submissionId);

    Task AttachFileToSubmissionByStudentId(
        Guid studentId,
        Guid submissionId,
        Guid fileId);

    Task DetachFileFromSubmissionByStudentId(
        Guid studentId,
        Guid submissionId,
        Guid fileId);

    Task<StudyFileDto> UploadSubmissionFileByStudentId(
        Guid studentId,
        Guid submissionId,
        IFormFile file);

    Task DeleteSubmissionFileByStudentId(
        Guid studentId,
        Guid submissionId,
        Guid fileId);
}

public class SubmissionService : ISubmissionService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IStudyFileService _studyFileService;

    public SubmissionService(StudyDbContext dbContext, IMapper mapper, IStudyFileService studyFileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _studyFileService = studyFileService;
    }

    // --------------
    //  CRUD METHODS
    // --------------
    public async Task<List<SubmissionDto>> GetAll()
    {
        var submissions = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .ToListAsync();

        return _mapper.Map<List<SubmissionDto>>(submissions);
    }

    public async Task<SubmissionDto> GetById(Guid submissionId)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException("Submission not found");

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task<SubmissionDto> Create(CreateSubmissionDto dto)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        var student = await _dbContext.Users
            .Include(u => u.EnrolledCourses)
            .FirstOrDefaultAsync(u => u.Id == dto.StudentId);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (student.Role != UserRole.Student)
            throw new InvalidOperationException("Only students can submit assignments");

        var isEnrolled = await _dbContext.CourseStudents
            .AnyAsync(cs =>
                cs.StudentId == student.Id &&
                cs.CourseId == assignment.CourseId);

        if (!isEnrolled)
            throw new SecurityException("Student is not enrolled in this course");

        var exists = await _dbContext.Submissions
            .AnyAsync(s =>
                s.AssignmentId == assignment.Id &&
                s.StudentId == student.Id);

        if (exists)
            throw new InvalidOperationException("Submission already exists");

        var submission = _mapper.Map<Submission>(dto);

        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();

        var result = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .FirstAsync(s => s.Id == submission.Id);

        return _mapper.Map<SubmissionDto>(result);
    }

    public async Task<SubmissionDto> Update(Guid submissionId, UpdateSubmissionDto dto)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException("Submission not found");

        submission.Comment = dto.Comment ?? submission.Comment;
        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task Delete(Guid submissionId)
    {
        var submission = await _dbContext.Submissions
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException("Submission not found");

        _dbContext.Submissions.Remove(submission);
        await _dbContext.SaveChangesAsync();
    }

    // ----------------
    // TEACHER METHODS
    // ----------------

    public async Task<List<SubmissionDto>> GetSubmissionsByAssignmentIdAndTeacherId(
        Guid teacherId,
        Guid assignmentId)
    {
        var submissions = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .Where(s =>
                s.AssignmentId == assignmentId &&
                s.Assignment.Course.TeacherId == teacherId)
            .ToListAsync();

        return _mapper.Map<List<SubmissionDto>>(submissions);
    }

    public async Task<SubmissionDto> GetSubmissionByTeacherId(
        Guid teacherId,
        Guid submissionId)
    {
        var submission = await VerifyTeacherSubmission(
            teacherId,
            submissionId);

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task<SubmissionDto> GradeSubmissionByTeacherId(
        Guid teacherId,
        Guid submissionId,
        GradeSubmissionRequest request)
    {
        var submission = await VerifyTeacherSubmission(
            teacherId,
            submissionId);

        VerifySubmissionGradeable(submission);
        
        submission.Grade = request.Grade;
        submission.Feedback = request.Feedback;
        submission.GradedAt = DateTime.UtcNow;
        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task<List<StudyFileDto>> GetSubmissionFilesByTeacherId(
        Guid teacherId,
        Guid submissionId)
    {
        var submission = await VerifyTeacherSubmission(
            teacherId,
            submissionId);

        return _mapper.Map<List<StudyFileDto>>(
            submission.StudyFiles);
    }

    // -----------------
    // STUDENT METHODS
    // -----------------

    // CRUD Methods
    public async Task<List<SubmissionDto>> GetSubmissionsByStudentId(Guid studentId)
    {
        var submissions = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .Where(s => s.StudentId == studentId)
            .ToListAsync();

        return _mapper.Map<List<SubmissionDto>>(submissions);
    }

    public async Task<SubmissionDto> GetSubmissionByStudentId(
        Guid studentId,
        Guid submissionId)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task<SubmissionDto> CreateSubmissionByStudentId(Guid studentId, CreateSubmissionDto dto)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        var isEnrolled = await _dbContext.CourseStudents
            .AnyAsync(cs =>
                cs.StudentId == studentId &&
                cs.CourseId == assignment.CourseId);

        if (!isEnrolled)
            throw new SecurityException("Student is not enrolled in this course");

        var existingSubmission = await _dbContext.Submissions
            .FirstOrDefaultAsync(s =>
                s.AssignmentId == assignment.Id &&
                s.StudentId == studentId);

        if (existingSubmission != null)
            throw new InvalidOperationException("Student already submitted this assignment");

        var submission = _mapper.Map<Submission>(dto);

        submission.SubmittedAt = DateTime.UtcNow;

        submission.Status =
            assignment.Deadline.HasValue &&
            assignment.Deadline < submission.SubmittedAt
                ? SubmissionStatus.SubmittedLate
                : SubmissionStatus.Submitted;

        submission.StudentId = studentId;

        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();
        
        submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .FirstAsync(s => s.Id == submission.Id);

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task<SubmissionDto> UpdateSubmissionByStudentId(
        Guid studentId,
        Guid submissionId,
        UpdateSubmissionDto dto)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);
        
        VerifySubmissionEditable(submission);
        

        submission.Comment = dto.Comment ?? submission.Comment;
        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task DeleteSubmissionByStudentId(Guid studentId, Guid submissionId)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);
        
        VerifySubmissionEditable(submission);

        _dbContext.Submissions.Remove(submission);

        await _dbContext.SaveChangesAsync();
    }

    // Student File Methods
    public async Task<List<StudyFileDto>> GetSubmissionFilesByStudentId(
        Guid studentId,
        Guid submissionId)
    {
        await VerifyStudentSubmission(studentId, submissionId);

        return await _studyFileService.GetFilesBySubmissionId(submissionId);
    }

    public async Task AttachFileToSubmissionByStudentId(
        Guid studentId,
        Guid submissionId,
        Guid fileId)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);
        
        VerifySubmissionEditable(submission);

        await _studyFileService.AttachToSubmission(
            fileId,
            submission.Id);

        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DetachFileFromSubmissionByStudentId(
        Guid studentId,
        Guid submissionId,
        Guid fileId)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);
        
        VerifySubmissionEditable(submission);

        await _studyFileService.RemoveFromSubmission(
            fileId,
            submission.Id);

        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<StudyFileDto> UploadSubmissionFileByStudentId(
        Guid studentId,
        Guid submissionId,
        IFormFile file)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);

        VerifySubmissionEditable(submission);

        var studyFile = await _studyFileService.UploadFile(
            file,
            studentId);

        await _studyFileService.AttachToSubmission(
            studyFile.Id,
            submission.Id);

        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<StudyFileDto>(studyFile);
    }

    public async Task DeleteSubmissionFileByStudentId(
        Guid studentId,
        Guid submissionId,
        Guid fileId)
    {
        var submission = await VerifyStudentSubmission(
            studentId,
            submissionId);
        
        VerifySubmissionEditable(submission);

        await _studyFileService.DeleteFile(
            fileId);
    }

    // -----------------
    // HELPER METHODS
    // -----------------
    private async Task<Submission> VerifyTeacherSubmission(
        Guid teacherId,
        Guid submissionId)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .ThenInclude(a => a.Course)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException(
                $"Submission with id {submissionId} not found.");

        if (submission.Assignment.Course.TeacherId != teacherId)
            throw new UnauthorizedAccessException(
                "Teacher does not have access to this submission.");

        return submission;
    }

    private async Task<Submission> VerifyStudentSubmission(
        Guid studentId,
        Guid submissionId)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .ThenInclude(a => a.Course)
            .Include(s => s.Student)
            .Include(s => s.StudyFiles)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException(
                $"Submission with id {submissionId} not found.");

        if (submission.StudentId != studentId)
            throw new UnauthorizedAccessException(
                "Student does not have access to this submission.");

        return submission;
    }
    
    
    private static void VerifySubmissionGradeable(
        Submission submission)
    {
        if (submission.Grade != null)
            throw new InvalidOperationException(
                "Submission has already been graded.");
    }
    
    private static void VerifySubmissionEditable(
        Submission submission)
    {
        if (submission.Grade != null)
            throw new InvalidOperationException(
                "Cannot modify a graded submission.");
    }
    
    
}