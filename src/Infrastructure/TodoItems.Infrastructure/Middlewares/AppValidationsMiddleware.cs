using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Infrastructure.Exceptions;
using TodoItems.Infrastructure.Extensions;

namespace TodoItems.Infrastructure.Middlewares;

public class AppValidationsMiddleware(RequestDelegate requestDelegate)
{
    private RequestDelegate RequestDelegate => requestDelegate;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await RequestDelegate(httpContext).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception).ConfigureAwait(false);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        using var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        ILogger logger = loggerFactory.CreateLogger<AppValidationsMiddleware>();

        var isDevelopment = httpContext.RequestServices.GetService<Microsoft.Extensions.Hosting.IHostEnvironment>()?.EnvironmentName == "Development";

        IAppResultError? result = null;

        var exceptionTypeName = exception.GetType().Name;

        HttpStatusCode statusCode = default;

        var message = string.Empty;

        switch (exception)
        {
            case DomainValidationException domainValidationException when exception is DomainValidationException:
                statusCode = HttpStatusCode.BadRequest;

                result = AppResult.Error(options =>
                {
                    options.WithTitle("Error de Validación de Dominio.")
                        .WithStatusCode(statusCode)
                        .WithExceptionType(exceptionTypeName)
                        .WithMessage(domainValidationException.Message);
                });

                logger.LogError(exception, "{StatusCode}", statusCode);
                break;

            case AppValidationsException validationException when exception is AppValidationsException:

                statusCode = HttpStatusCode.BadRequest;

                result = AppResult.Error(options =>
                {
                    options.WithTitle("Error de Validación de Request.")
                        .WithStatusCode(statusCode)
                        .WithExceptionType(exceptionTypeName)
                        .WithMessage(validationException.Message)
                        .WithErrors(validationException.Errors);
                });

                logger.LogError(exception, "{StatusCode}", statusCode);

                break;

            case UnauthorizedAccessException unauthorizedAccessException when exception is UnauthorizedAccessException:

                statusCode = HttpStatusCode.Unauthorized;

                result = AppResult.Error(options =>
                {
                    message = isDevelopment ? unauthorizedAccessException.Message : "Ocurrio un error de Autorización.";

                    options
                        .WithTitle("Error de Autorización.")
                        .WithStatusCode(statusCode)
                        .WithExceptionType(exceptionTypeName)
                        .WithMessage(message);
                });

                logger.LogError(exception, "{StatusCode}", statusCode);

                break;

            default:

                statusCode = HttpStatusCode.InternalServerError;

                result = AppResult.Error(options =>
                {
                    message = isDevelopment ? exception.Message : "Ocurrio un error inesperado, por favor contacte al administrador del sistema.";

                    options
                        .WithTitle("Error no controlado.")
                        .WithStatusCode(statusCode)
                        .WithExceptionType(exceptionTypeName)
                        .WithMessage(message);
                });

                logger.LogError(exception, "{StatusCode}", statusCode);

                break;
        }

        await httpContext.Response.ToResultAsync(
            status: statusCode,
            message: result.ToSerialize(x => x.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull),
            headers: new Dictionary<string, IEnumerable<string>>
            {
                { nameof(Exception), new List<string> { exceptionTypeName } },
            }).ConfigureAwait(false);
    }
}
