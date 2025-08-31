using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentBook.Api.Core.Features.Classes.Queries;

namespace StudentBook.Api.Core;

public static class Setup
{
    public static IHostApplicationBuilder AddCore(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(static configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ListClassesQuery).Assembly);
        });

        return builder;
    }
}