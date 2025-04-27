using Domain.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Common.Api.Attributes;

/// <summary>
/// Atributo para adicionar respostas padrão aos endpoints.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class DefaultResponseTypesAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Tipo do DTO de sucesso.
    /// </summary>
    public Type SuccessType { get; }

    /// <summary>
    /// Construtor para definir o tipo de sucesso.
    /// </summary>
    /// <param name="successType">Tipo do DTO de sucesso</param>
    public DefaultResponseTypesAttribute(Type successType)
    {
        SuccessType = successType;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var controllerActionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;

        if (controllerActionDescriptor != null)
        {
            context.HttpContext.Response.Headers.Add("X-Default-Response", "true"); // Apenas para depuração
            controllerActionDescriptor.EndpointMetadata.Add(new ProducesResponseTypeAttribute(SuccessType, 200));
            controllerActionDescriptor.EndpointMetadata.Add(new ProducesResponseTypeAttribute(typeof(BaseResponse<object>), 400));
            controllerActionDescriptor.EndpointMetadata.Add(new ProducesResponseTypeAttribute(typeof(BaseResponse<object>), 404));
            controllerActionDescriptor.EndpointMetadata.Add(new ProducesResponseTypeAttribute(typeof(BaseResponse<object>), 500));
        }

        base.OnActionExecuting(context);
    }
}