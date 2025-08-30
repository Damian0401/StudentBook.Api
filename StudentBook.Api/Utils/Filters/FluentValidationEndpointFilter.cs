using FluentValidation;

namespace StudentBook.Api.Utils.Filters;

public sealed class FluentValidationEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var services = context.HttpContext.RequestServices;
        var cancellationToken = context.HttpContext.RequestAborted;

        foreach (var argument in context.Arguments)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = services.GetService(validatorType);
            if (validator is not IValidator validatorInstance)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);
            var validationResult = await validatorInstance.ValidateAsync(validationContext, cancellationToken);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }
        }

        return await next(context);
    }
}