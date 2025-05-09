using System;
using System.Text;
using System.Text.Json.Serialization;
using Application.DI;
using Domain;
using Infrastructure.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Presentation.Common.Converters;

namespace Presentation.Common.Api;

public static class BuilderExtensions
{
   public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.IsDevelopment = builder.Environment.IsDevelopment();
        Configuration.JwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? string.Empty;
        Configuration.BackendUrl = Environment.GetEnvironmentVariable("BACKEND_URL") ?? string.Empty;
        Configuration.VersionApi = Environment.GetEnvironmentVariable("VERSION_API") ?? string.Empty;
        Configuration.PublicUrlFrontEnd = Environment.GetEnvironmentVariable("PUBLIC_URL_FRONTEND") ?? "http://domnum.com";
        Configuration.ApiKey = Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty;
        Configuration.ApiKeyAttribute = "X-API-KEY";
        Configuration.FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:4200";
        Configuration.SmtpUser = Environment.GetEnvironmentVariable("SMTP_USER") ?? string.Empty;
        Configuration.SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? string.Empty;
        Configuration.SmtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var smtpPort) ? smtpPort : 587;
        Configuration.SmtpPass = Environment.GetEnvironmentVariable("SMTP_PASS") ?? string.Empty;
        Configuration.RabbitMQUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? string.Empty;
        Configuration.RabbitMQHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? string.Empty;
        Configuration.RabbitMQPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? string.Empty;
        Configuration.AwsKeyId = Environment.GetEnvironmentVariable("AWS_KEY_ID") ?? string.Empty;
        Configuration.AwsKeySecret = Environment.GetEnvironmentVariable("AWS_KEY_SECRET") ?? string.Empty;
        Configuration.AwsRegion = Environment.GetEnvironmentVariable("AWS_REGION") ?? string.Empty;
        Configuration.BucketArchives = Environment.GetEnvironmentVariable("BUCKET_ARCHIVES") ?? string.Empty;
        Configuration.BucketVideos = Environment.GetEnvironmentVariable("BUCKET_VIDEOS") ?? string.Empty;

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.ReturnHttpNotAcceptable = true;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        field = e.Key,
                        errors = e.Value.Errors.Select(err => err.ErrorMessage)
                    });

                var result = new
                {
                    Message = "Invalid Request.",
                    Notifications = errors
                };
                return new BadRequestObjectResult(result);
            };
        })
        .AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            options.SerializerSettings.ContractResolver = new IgnoreEmptyEnumerablesContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.JwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        builder.Services.AddAuthorization();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                Configuration.CorsPolicyName,
                policy => policy
                    .WithOrigins([
                        Configuration.BackendUrl,
                        Configuration.FrontendUrl,
                        Configuration.FrontendUrl.Replace("http://", "https://"),
                    ])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            ));
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        if(builder.Environment.IsDevelopment())
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                // Personaliza os nomes dos esquemas para garantir unicidade
                options.CustomSchemaIds(type =>
                {
                    if (type.IsGenericType)
                    {
                        // Para tipos genéricos, exibe o nome base e os argumentos genéricos
                        var genericTypeName = type.GetGenericTypeDefinition().Name;
                        genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`')); // Remove o sufixo `1, `2, etc.

                        // Processa os argumentos genéricos recursivamente
                        var genericArgs = string.Join("_", type.GenericTypeArguments.Select(ProcessTypeName));
                        return $"{genericTypeName}_{genericArgs}";
                    }

                    // Para tipos não genéricos, inclui o namespace para garantir unicidade
                    return ProcessTypeName(type);
                });

                static string ProcessTypeName(Type type)
                {
                    if (type.IsGenericType)
                    {
                        // Para tipos genéricos, exibe o nome base e os argumentos genéricos
                        var genericTypeName = type.GetGenericTypeDefinition().Name;
                        genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`')); // Remove o sufixo `1, `2, etc.
                        var genericArgs = string.Join("_", type.GenericTypeArguments.Select(ProcessTypeName));
                        return $"{genericTypeName}_{genericArgs}";
                    }

                    // Para tipos não genéricos, verifica se "UseCases" está presente no nome completo
                    if (type.FullName != null && type.FullName.Contains("UseCases"))
                    {
                        return type.FullName
                            .Replace(".", "_")
                            .Replace("BaseResponse_", "")
                            .Substring(type.FullName.IndexOf("UseCases"));
                    }

                    // Caso "UseCases" não esteja presente, retorna o nome completo com ajustes
                    return type.FullName?.Replace(".", "_").Replace("BaseResponse_", "") ?? type.Name;
                }
            });
        }
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownProxies.Clear();
        });
        
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1024 * 1024 * 500;
        });
        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 1024 * 1024 * 500;
        });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });
        
        builder.Services.AppServices();
        builder.Services.AddRabbitMQ();
        builder.Services.ConfigureInfraServices();
    }
}