using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Create;

public record Request(
    string? Name, 
    string? Description,
    Stream? Imagem
) : IRequest<BaseResponse>;
