namespace StudentBook.Api.Endpoints.Classes.Models;

internal sealed record CreateClassDto
{
    public required string Name { get; init; }
    public required string LeadingTeacher { get; init; }
};