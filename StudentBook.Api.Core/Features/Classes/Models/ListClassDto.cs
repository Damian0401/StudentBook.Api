namespace StudentBook.Api.Core.Features.Classes.Models;

public sealed record ListClassDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string LeadingTeacher { get; init; }
};