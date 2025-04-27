using System;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.GetAll;


public record Request(int Skip, int Take) : IRequest<BaseResponse<List<Response>>>;