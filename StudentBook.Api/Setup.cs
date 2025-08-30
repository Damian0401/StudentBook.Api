using System.Globalization;
using FluentValidation;
using Scalar.AspNetCore;
using StudentBook.Api.Utils.Extensions;
using StudentBook.Api.Utils.Filters;

namespace StudentBook.Api;

internal static class Setup
{
    public static IHostApplicationBuilder AddApi(this IHostApplicationBuilder builder)
    {
        // OpenApi
        builder.Services.AddOpenApi(static options =>
        {
            options.ShouldInclude = _ => true;
        });

        // ApiVersioning
        builder.Services.AddEndpointsApiExplorer();
        builder.Services
            .AddApiVersioning()
            .AddApiExplorer(static options =>
            {
                options.SubstituteApiVersionInUrl = true;
            });

        // FluentValidation
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);

        // Endpoints
        builder.Services.AddApiEndpointsFromAssembly<Program>();

        // Other
        builder.Services.AddProblemDetails();

        return builder;
    }

    public static IEndpointRouteBuilder UseApi(this IEndpointRouteBuilder builder)
    {

        return builder;
    }

    public static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder builder)
    {
        // OpenApi
        builder.MapOpenApi();

        // Scalar
        builder.MapScalarApiReference("/docs", static options =>
        {
            options.HideClientButton = true;
            options.OperationSorter = OperationSorter.Alpha;
        });

        // ApiVersioning
        var api = builder.NewVersionedApi();
        var group = api.MapGroup("/api/v{version:apiVersion}");

        // FluentValidation
        group.AddEndpointFilter<FluentValidationEndpointFilter>();

        // ApiEndpoints
        builder.MapApiEndpoints(group);

        return builder;
    }
}