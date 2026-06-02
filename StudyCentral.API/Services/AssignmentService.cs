using StudyCentral.API.Models.Dtos.Assignments;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAssignmentService
{
    Task<Assignment> GetAssignment(Guid assignmentId);
    Task<List<Assignment>> GetAssignmentsByCourseId(Guid courseId);
    
    Task<Assignment> CreateAssignment(Guid teacherId, Guid courseId, CreateAssignmentDto request);
    Task<Assignment> UpdateAssignment(
        Guid teacherId,
        Guid assignmentId,
        UpdateAssignmentDto request
        );
    Task DeleteAssignment(Guid teacherId, Guid assignmentId);
    
}

public class AssignmentService : IAssignmentService
{
    public Task<Assignment> GetAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Assignment>> GetAssignmentsByCourseId(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<Assignment> CreateAssignment(Guid teacherId, Guid courseId, CreateAssignmentDto request)
    {
        throw new NotImplementedException();
    }

    public Task<Assignment> UpdateAssignment(Guid teacherId, Guid assignmentId, UpdateAssignmentDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAssignment(Guid teacherId, Guid assignmentId)
    {
        throw new NotImplementedException();
    }
}