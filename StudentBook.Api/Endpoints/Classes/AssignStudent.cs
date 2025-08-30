using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Utils.Abstraction;

namespace StudentBook.Api.Endpoints.Classes;

public sealed class AssignStudent : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Classes;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/classes/{classId:guid}/students/{studentId:guid}", HandleAsync);
    }

    private static async Task<Results<NoContent, ValidationProblem>> HandleAsync(
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
            this.RuleFor(x => x.ClassId)
                .NotEmpty();
            this.RuleFor(x => x.StudentId)
                .NotEmpty();
        }
    }

    internal readonly struct Parameters
    {
        [FromRoute]
        public required Guid ClassId { get; init; }

        [FromRoute]
        public required Guid StudentId { get; init; }
    }

    internal readonly struct Services
    {
        public required CancellationToken CancellationToken { get; init; }
    }
}