namespace StudentBook.Api.Core.Features.Students.Models;

public sealed record CreateStudentDto
{
    public required string Identifier { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateOnly? DateOfBirth { get; init; }
    public string? City { get; init; }
    public string? Street { get; init; }
    public string? PostalCode { get; init; }
};