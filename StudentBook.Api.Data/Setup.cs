using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentBook.Api.Common.Constants;

namespace StudentBook.Api.Data;

public static class Setup
{
    public static IHostApplicationBuilder AddData(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<StudentBookDb>(options =>
        {
            options.UseSqlite(
                builder.Configuration
                    .GetRequiredSection(Config.ConnectionStrings.Database)
                    .Value);
        });

        return builder;
    }
}