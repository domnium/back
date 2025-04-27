namespace Application.UseCases.Lecture.Get.AllCourseCompleted;

public record Response
{
    public Guid? Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? VideoUrl { get; init; }
    public DateTime? CompletionDate { get; init; }
}