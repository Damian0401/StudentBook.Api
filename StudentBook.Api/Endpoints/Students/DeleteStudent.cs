using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Core.Features.Students.Commands;
using StudentBook.Api.Utils.Abstraction;
using StudentBook.Api.Utils.Extensions;

namespace StudentBook.Api.Endpoints.Students;

public sealed class DeleteStudent : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Students;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/students/{studentId:guid}", HandleAsync);
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
        }
    }

    internal readonly struct Parameters
    {
        [FromRoute]
        public required Guid StudentId { get; init; }

        internal DeleteStudentCommand ToCommand()
        {
            return new()
            {
                StudentId = this.StudentId,
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