using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.IA.Create;

public record Request(
    string Name, 
    IFormFile Picture
) : IRequest<BaseResponse>;