namespace StudentBook.Api.Endpoints.Students.Models;

internal sealed record UpdateStudentDto
{
    public string? Identifier { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? PostalCode { get; init; }
};
