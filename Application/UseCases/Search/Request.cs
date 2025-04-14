using Domain.Records;
using MediatR;

namespace Application.UseCases.Search;

public record Request(
    string Query,
    int? Page = 0,
    int? PageSize = 10
) : IRequest<BaseResponse>;