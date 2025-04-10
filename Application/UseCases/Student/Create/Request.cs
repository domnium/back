using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Student.Create;

public record Request(
    string? Name,
    Guid UserId,
    bool IsFreeStudent,
    IFormFile? Picture
) : IRequest<BaseResponse>;
