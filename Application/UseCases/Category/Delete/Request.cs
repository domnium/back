using System;
using Application.UseCases.Category.GetById;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Delete;

public record Request(Guid? Id) 
    : GetById.Request(Id), IRequest<BaseResponse>;

