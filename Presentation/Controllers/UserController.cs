using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Api;
using Presentation.Middlewares;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

using LoginRequest = Application.UseCases.User.Login.Request;
using RegisterRequest = Application.UseCases.User.Register.Request;
using ActivateRequest = Application.UseCases.User.Activate.Request;
using ActivatePasswordRequest = Application.UseCases.User.ForgotPassword.Activate.Request;
using ForgotRequest = Application.UseCases.User.ForgotPassword.Request;
using ResendCodeRequest = Application.UseCases.User.ResendCode.Request;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável pelas operações de autenticação e gerenciamento de usuários,
/// como login, registro, ativação de conta, recuperação e redefinição de senha.
/// </summary>
[ApiController]
[Route("user")]
public class UserController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Realiza o login do usuário com e-mail e senha.
    /// </summary>
    /// <param name="request">Credenciais de login</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Token de acesso ou erro de autenticação</returns>
    [HttpPost("login")]
    [ApiKey]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, response);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    /// <summary>
    /// Realiza o cadastro de um novo usuário e envia código de ativação por e-mail.
    /// </summary>
    /// <param name="request">Dados de registro do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação e notificações</returns>
    [HttpPost("register")]
    [ApiKey]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new { response.statuscode, response.message, response.notifications });
        }
        catch (SmtpException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    /// <summary>
    /// Ativa uma conta de usuário por e-mail e código.
    /// </summary>
    /// <param name="code">Código de ativação</param>
    /// <param name="email">E-mail do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da ativação</returns>
    [HttpPut("activate")]
    [ApiKey]
    public async Task<IActionResult> Activate([FromQuery] string code, [FromQuery] string email, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var response = await mediator.Send(
                new ActivateRequest(email, long.TryParse(code, out var tokenlong) ? tokenlong : 0),
                cancellationToken);

            return StatusCode(response.statuscode, response);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    /// <summary>
    /// Inicia o processo de recuperação de senha (envia código de redefinição por e-mail).
    /// </summary>
    /// <param name="request">Dados do usuário (e-mail)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da solicitação</returns>
    [HttpPut("forgot-password")]
    [ApiKey]
    public async Task<IActionResult> ForgotPassword(ForgotRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new { response.statuscode, response.message, response.notifications });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    /// <summary>
    /// Reenvia o código de ativação para o e-mail do usuário.
    /// </summary>
    /// <param name="request">Dados do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação</returns>
    [HttpPut("resend-code")]
    [ApiKey]
    public async Task<IActionResult> ResendCode(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new { response.statuscode, response.message, response.notifications });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    /// <summary>
    /// Ativa uma nova senha após processo de recuperação.
    /// </summary>
    /// <param name="request">Dados do código e nova senha</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da redefinição</returns>
    [HttpPut("forgot-password/activate")]
    [ApiKey]
    public async Task<IActionResult> ForgotPasswordActivate(ActivatePasswordRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.notifications });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
}
