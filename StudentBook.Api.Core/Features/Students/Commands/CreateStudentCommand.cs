using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Core.Features.Students.Models;
using StudentBook.Api.Data.Entities;
using StudentBook.Api.Data.Repositories.Student;

namespace StudentBook.Api.Core.Features.Students.Commands;

public sealed record CreateStudentCommand : IRequest<Result>
{
    public required CreateStudentDto Student { get; init; }
}

internal class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result>
{
    private readonly IStudentRepository studentRepository;

    public CreateStudentCommandHandler(IStudentRepository studentRepository)
    {
        this.studentRepository = studentRepository;
    }

    public async Task<Result> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var errors = await this.ValidateAsync(request, cancellationToken);
        if (errors.Count != 0)
        {
            var errorsAsDictionary = errors
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, x => x.ToArray());
            return Result.Failure(ResultErrorType.Invalid, errorsAsDictionary);
        }

        var entity = new StudentEntity
        {
            Identifier = request.Student.Identifier,
            FirstName = request.Student.FirstName,
            LastName = request.Student.LastName,
            DateOfBirth = request.Student.DateOfBirth.Value,
            City = request.Student.City,
            Street = request.Student.Street,
            PostalCode = request.Student.PostalCode,
        };
        await this.studentRepository.CreateAsync(entity, cancellationToken);
        return Result.Success();
    }

    private async Task<IReadOnlyCollection<KeyValuePair<string, string>>> ValidateAsync(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        List<KeyValuePair<string, string>> errors = [];
        var identifierAlreadyExists = await this.studentRepository.IdentifierExistsAsync(
            request.Student.Identifier,
            cancellationToken);

        if (identifierAlreadyExists)
        {
            var errorMessage = $"Student with identifier {request.Student.Identifier} already exists.";
            errors.Add(new(nameof(request.Student.Identifier), errorMessage));
        }
        return errors;
    }
}