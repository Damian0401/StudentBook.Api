using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Utils.Abstraction;

namespace StudentBook.Api.Endpoints.Students;

public sealed class ListStudents : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Students;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/students", HandleAsync);
    }

    private static async Task<Results<Ok, ValidationProblem>> HandleAsync(
        [AsParameters] Parameters parameters,
        [AsParameters] Services services)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    internal class Validator : AbstractValidator<Parameters>
    {
        public Validator()
        {
            this.RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Page.HasValue);
            this.RuleFor(x => x.Size)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(Utils.Constants.Pagination.MaxSize)
                .When(x => x.Size.HasValue);
        }
    }

    internal readonly struct Parameters : IPaginationParams
    {
        [FromQuery]
        public Guid? ClassId { get; init; }

        [FromQuery]
        public int? Page { get; init; }

        [FromQuery]
        public int? Size { get; init; }
    }

    internal readonly struct Services
    {
        public required CancellationToken CancellationToken { get; init; }
    }
}