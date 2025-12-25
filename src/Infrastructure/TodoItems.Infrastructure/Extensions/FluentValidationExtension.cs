using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Reflection;
using TodoItems.Infrastructure.Exceptions;

namespace TodoItems.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
    {
        var typesInNamespace = assembly
            .GetTypes()
            .Where(
                type =>
                    type.Namespace is not null
                    && !type.IsAbstract
                    && !type.IsInterface
                    && type.Name.EndsWith("Validator")
            );

       foreach (var type in typesInNamespace)
        {
            var validatorInterface = type.GetInterfaces()
                .FirstOrDefault(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)
                );

            if (validatorInterface is not null)
            {
                var genericArgument = validatorInterface.GetGenericArguments()[0];
                var serviceType = typeof(IValidator<>).MakeGenericType(genericArgument);
                services.AddScoped(serviceType, type);
            }
        }

        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableBuiltInModelValidation = true;
            config.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });

        return services;
    }

    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    key => key.Key,
                    value => value.Value!.Errors.Select(e => e.ErrorMessage));


            throw new AppValidationsException(errors);
       }
    }
}
