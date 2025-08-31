using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentBook.Api.Common.Constants;
using StudentBook.Api.Data.Repositories.Class;
using StudentBook.Api.Data.Repositories.Student;

namespace StudentBook.Api.Data;

public static class Setup
{
    public static IHostApplicationBuilder AddData(this IHostApplicationBuilder builder)
    {
        // DbContext
        builder.Services.AddDbContext<StudentBookDb>(options =>
        {
            options.UseSqlite(
                builder.Configuration
                    .GetRequiredSection(Config.ConnectionStrings.Database)
                    .Value);
        });

        // Repositories
        builder.Services.AddTransient<IClassRepository, ClassRepository>();
        builder.Services.AddTransient<IStudentRepository, StudentRepository>();

        return builder;
    }
}