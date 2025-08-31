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

public sealed class UpdateStudent : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Students;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut("/students/{studentId:guid}", HandleAsync);
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
            this.RuleFor(x => x.StudentId)
                .NotEmpty();
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
                .NotEmpty()
                .MaximumLength(256)
                .When(x => x.Student.City is not null);
            this.RuleFor(x => x.Student.Street)
                .NotEmpty()
                .MaximumLength(256)
                .When(x => x.Student.Street is not null);
            this.RuleFor(x => x.Student.PostalCode)
                .NotEmpty()
                .MaximumLength(256)
                .When(x => x.Student.PostalCode is not null);
        }
    }

    internal readonly struct Parameters
    {
        [FromRoute]
        public required Guid StudentId { get; init; }

        [FromBody]
        public required UpdateStudentDto Student { get; init; }

        internal UpdateStudentCommand ToCommand()
        {
            return new()
            {
                StudentId = this.StudentId,
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