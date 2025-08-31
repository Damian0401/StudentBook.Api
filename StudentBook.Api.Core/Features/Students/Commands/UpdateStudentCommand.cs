using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Core.Features.Students.Models;
using StudentBook.Api.Data.Entities;
using StudentBook.Api.Data.Repositories.Student;

namespace StudentBook.Api.Core.Features.Students.Commands;

public sealed record UpdateStudentCommand : IRequest<Result>
{
    public required Guid StudentId { get; init; }

    public required UpdateStudentDto Student { get; init; }
}

internal class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Result>
{
    private readonly IStudentRepository studentRepository;

    public UpdateStudentCommandHandler(IStudentRepository studentRepository)
    {
        this.studentRepository = studentRepository;
    }

    public async Task<Result> Handle(
        UpdateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var studentToUpdate = await this.studentRepository.GetByIdAsync(
            request.StudentId,
            cancellationToken);
        var validationResult = await this.ValidateAsync(studentToUpdate, request, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        studentToUpdate.Identifier = request.Student.Identifier;
        studentToUpdate.FirstName = request.Student.FirstName;
        studentToUpdate.LastName = request.Student.LastName;
        studentToUpdate.DateOfBirth = request.Student.DateOfBirth.Value;
        studentToUpdate.City = request.Student.City;
        studentToUpdate.Street = request.Student.Street;
        studentToUpdate.PostalCode = request.Student.PostalCode;

        await this.studentRepository.UpdateAsync(studentToUpdate, cancellationToken);
        return Result.Success();
    }

    private async Task<Result> ValidateAsync(
        StudentEntity? studentToUpdate,
        UpdateStudentCommand request,
        CancellationToken cancellationToken)
    {
        if (studentToUpdate is null)
        {
            return Result.Failure(ResultErrorType.NotFound);
        }

        var identifierAlreadyExists = await this.studentRepository.IdentifierExistsAsync(
            request.Student.Identifier,
            cancellationToken);
        if (identifierAlreadyExists)
        {
            var errorMessage = $"Student with identifier {request.Student.Identifier} already exists.";
            return Result.Failure(ResultErrorType.Invalid, new Dictionary<string, string[]>
            {
                {
                    nameof(request.Student.Identifier),
                    [errorMessage]
                },
            });
        }

        return Result.Success();
    }
}