using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Core.Features.Classes.Commands;
using StudentBook.Api.Core.Features.Classes.Models;
using StudentBook.Api.Utils.Abstraction;
using StudentBook.Api.Utils.Extensions;

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

        internal CreateClassCommand ToCommand()
        {
            return new()
            {
                Class = this.Class,
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