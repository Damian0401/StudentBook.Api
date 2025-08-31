using MediatR;
using StudentBook.Api.Core.Features.Students.Models;
using StudentBook.Api.Core.Utils;
using StudentBook.Api.Data;

namespace StudentBook.Api.Core.Features.Students.Queries;

public sealed record ListStudentsQuery
    : IRequest<PaginatedResponseDto<ListStudentDto>>, IPaginatedQuery
{
    public required int? Page { get; init; }
    public required int? Size { get; init; }
    public required Guid? ClassId { get; init; }
}

internal sealed class ListStudentsQueryHandler : IRequestHandler<ListStudentsQuery, PaginatedResponseDto<ListStudentDto>>
{
    private readonly StudentBookDb studentBookDb;

    public ListStudentsQueryHandler(StudentBookDb studentBookDb)
    {
        this.studentBookDb = studentBookDb;
    }

    public async Task<PaginatedResponseDto<ListStudentDto>> Handle(
        ListStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var studentsQuery = this.studentBookDb.Students
            .Select(s => new ListStudentDto
            {
                Id = s.Id,
                Identifier = s.Identifier,
                FirstName = s.FirstName,
                LastName = s.LastName,
                DateOfBirth = s.DateOfBirth,
                ClassId = s.ClassId,
                City = s.City,
                Street = s.Street,
                PostalCode = s.PostalCode,
            });

        if (request.ClassId is not null)
        {
            studentsQuery = studentsQuery
                .Where(s => s.ClassId == request.ClassId);
        }

        var result = await PaginatedResponseDto.FromAsync(
            studentsQuery,
            request,
            cancellationToken);

        return result;
    }
}