namespace StudentBook.Api.Core.Features.Students.Models;

public sealed record ListStudentDto
{
    public required Guid Id { get; init; }
    public required string Identifier { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required Guid? ClassId { get; init; }
    public required string? City { get; init; }
    public required string? Street { get; init; }
    public required string? PostalCode { get; init; }
}