using Asp.Versioning;

namespace StudentBook.Api.Abstraction;

internal interface IApiEndpoint
{
    ApiVersion Version { get; }
    string DefaultTag { get; }
    IEndpointConventionBuilder Register(IEndpointRouteBuilder builder);
}