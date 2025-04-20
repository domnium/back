using System;
using Domain.Records;
using MediatR;

namespace Application.UseCases.IA.Delete;

public record Request(
    Guid Id
) : IRequest<BaseResponse>;
