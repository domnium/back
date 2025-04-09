using System;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.GetAll;

public record Request : IRequest<BaseResponse>;
