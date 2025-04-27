namespace Application.UseCases.IA.Get.All;

public record Response
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public string? PictureUrl { get; init; }
}