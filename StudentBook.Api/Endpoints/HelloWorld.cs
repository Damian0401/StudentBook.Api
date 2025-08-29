using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Abstraction;

namespace StudentBook.Api.Endpoints;

internal sealed class HelloWorld : IApiEndpoint
{
    public ApiVersion Version => Constants.Api.Versions.V1;
    public string DefaultTag => Constants.Api.Tags.Hello;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/hello", HandleAsync);
    }

    private static async Task<Ok<string>> HandleAsync(
        [AsParameters] Parameters parameters)
    {
        await Task.CompletedTask;
        return TypedResults.Ok($"Hello {parameters.Name}!");
    }
    
    private readonly struct Parameters
    {
        [FromQuery]
        public required string Name { get; init; } 
    }
}