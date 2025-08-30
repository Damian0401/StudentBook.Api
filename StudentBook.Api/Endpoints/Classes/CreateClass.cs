using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Endpoints.Classes.Models;
using StudentBook.Api.Utils.Abstraction;

namespace StudentBook.Api.Endpoints.Classes;

public sealed class CreateClass : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Classes;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/classes", HandleAsync);
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
            this.RuleFor(x => x.Class.Name)
                .NotEmpty()
                .MaximumLength(256);
            this.RuleFor(x => x.Class.LeadingTeacher)
                .NotEmpty()
                .MaximumLength(256);
        }
    }

    internal readonly struct Parameters
    {
        [FromBody]
        public required CreateClassDto Class { get; init; }
    }

    internal readonly struct Services
    {
        public required CancellationToken CancellationToken { get; init; }
    }
}