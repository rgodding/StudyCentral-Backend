using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.DTOs.Assignment;

public class StudentAssignmentDto : AssignmentDto
{
    public SubmissionStatus SubmissionStatus { get; set; }
}