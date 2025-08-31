namespace StudentBook.Api.Core.Features.Classes.Models;

public sealed record UpdateClassDto
{
    public required string Name { get; init; }
    public required string LeadingTeacher { get; init; }
};
