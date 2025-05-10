using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ForgotPassword.Activate;

public record Request(Guid? token, string newPassword) : IRequest<BaseResponse<object>>;