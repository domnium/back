namespace Application.UseCases.Course.Get.ByCategory;

public record Response
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public int? Subscribes { get; init; }
    public string? Description { get; init; }
    public string? AboutDescription { get; init; }
    public string? ImageUrl { get; init; }
    public string? TrailerUrl { get; init; }
    public string? TeacherName { get; init; }
    public string? TeacherPictureUrl { get; init; }
}