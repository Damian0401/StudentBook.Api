using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StudentBook.Api.Common;

public static class Setup
{
    public static IHostApplicationBuilder AddCommon(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        return builder;
    }
}