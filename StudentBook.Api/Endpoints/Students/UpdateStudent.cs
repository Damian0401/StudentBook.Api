using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Endpoints.Students.Models;
using StudentBook.Api.Utils.Abstraction;

namespace StudentBook.Api.Endpoints.Students;

public sealed class UpdateStudent : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Students;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut("/students/{studentId:guid}", HandleAsync);
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
            this.RuleFor(x => x.StudentId)
                .NotEmpty();
            this.RuleFor(x => x.Student.Identifier)
                .MaximumLength(256);
            this.RuleFor(x => x.Student.FirstName)
                .MaximumLength(256);
            this.RuleFor(x => x.Student.LastName)
                .MaximumLength(256);
            this.RuleFor(x => x.Student.City)
                .MaximumLength(256);
            this.RuleFor(x => x.Student.Street)
                .MaximumLength(256);
            this.RuleFor(x => x.Student.PostalCode)
                .MaximumLength(256);
        }
    }

    internal readonly struct Parameters
    {
        [FromRoute]
        public required Guid StudentId { get; init; }

        [FromBody]
        public required UpdateStudentDto Student { get; init; }
    }

    internal readonly struct Services
    {
        public required CancellationToken CancellationToken { get; init; }
    }
}