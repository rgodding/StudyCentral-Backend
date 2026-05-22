namespace StudyCentral.API.Models.ApiModels.CourseModels;

public class CreateCourseRequestModel
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
}