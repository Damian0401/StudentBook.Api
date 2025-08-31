namespace StudentBook.Api.Core.Features.Classes.Models;

public sealed record CreateClassDto
{
    public required string Name { get; init; }
    public required string LeadingTeacher { get; init; }
};