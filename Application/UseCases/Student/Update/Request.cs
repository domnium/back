using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Student.Update;

public record Request(
    Guid StudentId,
    string? Name,
    bool IsFreeStudent,
    IFormFile? Picture
) : IRequest<BaseResponse>;
