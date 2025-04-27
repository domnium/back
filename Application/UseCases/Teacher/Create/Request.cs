using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Teacher.Create;

public record Request(
    string Name,
    string Email,
    string Cpf,
    string Phone,
    string Endereco,
    string Cep,
    string? Tiktok,
    string? Instagram,
    string? GitHub,
    string Description,
    IFormFile Picture
) : IRequest<BaseResponse<object>>;
