using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Core.Features.Classes.Commands;
using StudentBook.Api.Utils.Abstraction;
using StudentBook.Api.Utils.Extensions;

namespace StudentBook.Api.Endpoints.Classes;

public sealed class UnassignStudentFromClass : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Classes;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/classes/{classId:guid}/students/{studentId:guid}", HandleAsync);
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

        internal UnassignStudentFromClassCommand ToCommand()
        {
            return new()
            {
                ClassId = this.ClassId,
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