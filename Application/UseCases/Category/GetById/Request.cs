using System;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.GetById;

public record Request(
    Guid? Id
)
: IRequest<BaseResponse>;
