using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Course.Create;

public record Request(
    string Name,
    string Description,
    string AboutDescription,
    IFormFile Picture, 
    string? GitHubUrl,
    string NotionUrl,
    Guid IAId,
    Guid CategoryId,
    IFormFile? Trailer,
    Guid? ParametersId,
    Guid TeacherId,
    decimal Price,
    decimal TotalHours
) 
: IRequest<BaseResponse>;
