using Scalar.AspNetCore;

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
        
        var api = builder.NewVersionedApi();
        var group = api.MapGroup("/api/v{version:apiVersion}");
        
        group.MapGet("/hello", () =>  "Hello World!")
            .WithTags(Constants.Api.Tags.Hello)
            .HasApiVersion(Constants.Api.Versions.V1);
        
        group.MapGet("/world", () =>  "Hello World!")
            .WithTags(Constants.Api.Tags.World)
            .HasApiVersion(Constants.Api.Versions.V1);
        
        return builder;
    }
}