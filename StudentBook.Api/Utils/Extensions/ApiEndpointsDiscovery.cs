using Microsoft.Extensions.DependencyInjection.Extensions;
using StudentBook.Api.Utils.Abstraction;

namespace StudentBook.Api.Utils.Extensions;

public static class ApiEndpointsDiscovery
{
    public static IServiceCollection AddApiEndpointsFromAssembly<TAssembly>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var types = typeof(TAssembly)
            .Assembly
            .DefinedTypes
            .Where(x =>
                x is {IsAbstract: false, IsInterface: false} &&
                x.IsAssignableTo(typeof(IApiEndpoint)))
            .Select(t =>
                ServiceDescriptor.Describe(typeof(IApiEndpoint), t, lifetime));
        services.TryAddEnumerable(types);
        return services;
    }

    public static RouteGroupBuilder MapApiEndpoints(
        this IEndpointRouteBuilder builder,
        RouteGroupBuilder group)
    {
        var endpoints = builder.ServiceProvider.GetServices<IApiEndpoint>();
        foreach (var endpoint in endpoints)
        {
            endpoint.Register(group)
                .HasApiVersion(endpoint.Version)
                .WithTags(endpoint.DefaultTag);
        }
        return group;
    }
}