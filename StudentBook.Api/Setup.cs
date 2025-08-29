using Scalar.AspNetCore;
using StudentBook.Api.Extensions;

namespace StudentBook.Api;

public static class Setup
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
        
        // Endpoints
        builder.Services.AddApiEndpointsFromAssembly<Program>();

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
        
        // ApiEndpoints
        builder.MapApiEndpoints(group);

        return builder;
    }
}