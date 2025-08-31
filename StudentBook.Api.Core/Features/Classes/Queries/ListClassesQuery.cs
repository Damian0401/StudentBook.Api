using MediatR;
using StudentBook.Api.Core.Features.Classes.Models;
using StudentBook.Api.Core.Utils;
using StudentBook.Api.Data;

namespace StudentBook.Api.Core.Features.Classes.Queries;

public sealed record ListClassesQuery
    : IRequest<PaginatedResponseDto<ListClassDto>>, IPaginatedQuery
{
    public required int? Page { get; init; }
    public required int? Size { get; init; }
}

internal sealed class ListClassesQueryHandler : IRequestHandler<ListClassesQuery, PaginatedResponseDto<ListClassDto>>
{
    private readonly StudentBookDb studentBookDb;

    public ListClassesQueryHandler(StudentBookDb studentBookDb)
    {
        this.studentBookDb = studentBookDb;
    }

    public async Task<PaginatedResponseDto<ListClassDto>> Handle(
        ListClassesQuery request,
        CancellationToken cancellationToken)
    {
        var classesQuery = this.studentBookDb.Classes
            .Select(s => new ListClassDto
            {
                Id = s.Id,
                Name = s.Name,
                LeadingTeacher = s.LeadingTeacher.Name,
            });

        var result = await PaginatedResponseDto.FromAsync(
            classesQuery,
            request,
            cancellationToken);

        return result;
    }
}