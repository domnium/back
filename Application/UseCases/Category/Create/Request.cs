using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Category.Create;

public record Request(
    string Name, 
    string Description,
    IFormFile Imagem
) : IRequest<BaseResponse<object>>;
