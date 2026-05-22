namespace StudyCentral.API.Models.ApiModels.TeacherModels;

public class UpdateAssignmentRequestModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
}