using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBook.Api.Common.Constants;
using StudentBook.Api.Core.Features.Classes.Models;
using StudentBook.Api.Core.Features.Classes.Queries;
using StudentBook.Api.Core.Utils;
using StudentBook.Api.Utils.Abstraction;

namespace StudentBook.Api.Endpoints.Classes;

public sealed class ListClasses : IApiEndpoint
{
    public ApiVersion Version => Utils.Constants.Api.Versions.V1;
    public string DefaultTag => Utils.Constants.Api.Tags.Classes;
    public IEndpointConventionBuilder Register(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/classes", HandleAsync);
    }

    private static async Task<Results<Ok<PaginatedResponseDto<ListClassDto>>, ValidationProblem>> HandleAsync(
        [AsParameters] Parameters parameters,
        [AsParameters] Services services)
    {
        var response = await services.Sender.Send(
            parameters.ToQuery(),
            services.CancellationToken);
        return TypedResults.Ok(response);
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
                .LessThanOrEqualTo(Pagination.MaxSize)
                .When(x => x.Size.HasValue);
        }
    }

    internal readonly struct Parameters : IPaginationParams
    {
        [FromQuery]
        public int? Page { get; init; }

        [FromQuery]
        public int? Size { get; init; }

        internal ListClassesQuery ToQuery()
        {
            return new()
            {
                Page = this.Page,
                Size = this.Size,
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