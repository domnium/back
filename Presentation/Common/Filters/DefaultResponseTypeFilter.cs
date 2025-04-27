using System.Linq;
using Microsoft.OpenApi.Models;
using Presentation.Common.Api.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Common.Filters;

/// <summary>
/// Filtro para adicionar respostas padrão aos endpoints.
/// </summary>
public class DefaultResponseTypeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica se o atributo DefaultResponseTypesAttribute está presente
        var defaultResponseAttribute = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<DefaultResponseTypesAttribute>()
            .FirstOrDefault();

        if (defaultResponseAttribute != null)
        {
            // Adiciona o tipo de sucesso (200)
            operation.Responses.TryAdd("200", new OpenApiResponse
            {
                Description = "Success",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(defaultResponseAttribute.SuccessType, context.SchemaRepository)
                    }
                }
            });
        }

        // Adiciona respostas padrão
        operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });
        operation.Responses.TryAdd("404", new OpenApiResponse { Description = "Not Found" });
        operation.Responses.TryAdd("409", new OpenApiResponse { Description = "Conflict" });
        operation.Responses.TryAdd("500", new OpenApiResponse { Description = "Internal Server Error" });
    }
}