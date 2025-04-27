using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Activate;

/// <summary>
/// Representa a requisição para ativar um usuário.
/// </summary>
public record Request(
    string email,
    long token
) : IRequest<BaseResponse<object>>;