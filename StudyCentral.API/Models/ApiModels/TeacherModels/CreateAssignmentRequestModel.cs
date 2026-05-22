namespace StudyCentral.API.Models.ApiModels.TeacherModels;

public class CreateAssignmentRequestModel
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
}