using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Lecture.Create;

public record Request(
    Guid Moduleid ,
    string Name ,
    string Tempo,
    string NotionUrl, 
    IFormFile File 
) : IRequest<BaseResponse>;
