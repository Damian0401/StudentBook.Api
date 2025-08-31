using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Core.Features.Students.Commands;
using StudentBook.Api.Core.Features.Students.Models;
using StudentBook.Api.Utils.Abstraction;
using StudentBook.Api.Utils.Extensions;

namespace StudentBook.Api.Endpoints.Students;

public sealed class CreateStudent : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Students;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/students", HandleAsync);
    }

    private static async Task<Results<NoContent, NotFound, ValidationProblem>> HandleAsync(
        [AsParameters] Parameters parameters,
        [AsParameters] Services services)
    {
        var result = await services.Sender.Send(
            parameters.ToCommand(),
            services.CancellationToken);
        return result.ResolveStatusCode();
    }

    internal class Validator : AbstractValidator<Parameters>
    {
        public Validator()
        {
            this.RuleFor(x => x.Student.Identifier)
                .NotEmpty()
                .MaximumLength(256);
            this.RuleFor(x => x.Student.FirstName)
                .NotEmpty()
                .MaximumLength(256);
            this.RuleFor(x => x.Student.LastName)
                .NotEmpty()
                .MaximumLength(256);
            this.RuleFor(x => x.Student.DateOfBirth)
                .NotEmpty();
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
        [FromBody]
        public required CreateStudentDto Student { get; init; }

        internal CreateStudentCommand ToCommand()
        {
            return new()
            {
                Student = this.Student,
            };
        }
    }

    internal readonly struct Services
    {
        [FromServices]
        public required ISender Sender { get; init; }

        public required CancellationToken CancellationToken { get; init; }
    }
}