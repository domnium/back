using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Api;
using Presentation.Middlewares;

using LoginRequest = Application.UseCases.User.Login.Request;
using RegisterRequest = Application.UseCases.User.Register.Request;
using ActivateRequest = Application.UseCases.User.Activate.Request;
using ActivatePasswordRequest = Application.UseCases.User.ForgotPassword.Activate.Request;
using ForgotRequest = Application.UseCases.User.ForgotPassword.Request;
using ResendCodeRequest = Application.UseCases.User.ResendCode.Request;

using LoginResponse = Application.UseCases.User.Login.Response;

using Swashbuckle.AspNetCore.Annotations;
using Domain.Records;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável pelas operações de autenticação e gerenciamento de usuários.
/// </summary>
[ApiController]
[Route("User")]
public class UserController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Realiza o login do usuário com e-mail e senha.
    /// </summary>
    [HttpPost("Login")]
    [SwaggerOperation(OperationId = "UserLogin")]
    [ApiKey]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 403)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response.Token);
    }

    /// <summary>
    /// Realiza o cadastro de um novo usuário e envia código de ativação por e-mail.
    /// </summary>
    [HttpPost("Register")]
    [SwaggerOperation(OperationId = "UserRegister")]
    [ApiKey]
    [ProducesResponseType(typeof(BaseResponse<object>), 201)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, new { response.StatusCode, response.Message, response.Notifications });
    }

    /// <summary>
    /// Ativa uma conta de usuário por e-mail e código.
    /// </summary>
    [HttpPut("Activate")]
    [SwaggerOperation(OperationId = "UserActivate")]
    [ApiKey]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Activate([FromQuery] string code, [FromQuery] string email, CancellationToken cancellationToken)
    {
        var tokenlong = long.TryParse(code, out var parsedToken) ? parsedToken : 0;
        var response = await mediator.Send(new ActivateRequest(email, tokenlong), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Inicia o processo de recuperação de senha.
    /// </summary>
    [HttpPut("Forgot-Password")]
    [SwaggerOperation(OperationId = "UserForgotPassword")]
    [ApiKey]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, new { response.StatusCode, response.Message, response.Notifications });
    }

    /// <summary>
    /// Reenvia o código de ativação para o e-mail do usuário.
    /// </summary>
    [HttpPut("Resend-Code")]
    [SwaggerOperation(OperationId = "UserResendCode")]
    [ApiKey]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, new { response.StatusCode, response.Message, response.Notifications });
    }

    /// <summary>
    /// Ativa uma nova senha após processo de recuperação.
    /// </summary>
    [HttpPut("Forgot-Password/Activate")]
    [SwaggerOperation(OperationId = "UserForgotPasswordActivate")]
    [ApiKey]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> ForgotPasswordActivate([FromBody] ActivatePasswordRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, new { response.Message, response.Notifications });
    }
}