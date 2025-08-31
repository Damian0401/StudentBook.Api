using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Data.Repositories.Class;

namespace StudentBook.Api.Core.Features.Classes.Commands;

public sealed record AssignStudentToClassCommand : IRequest<Result>
{
    public required Guid ClassId { get; init; }
    public required Guid StudentId { get; init; }
}

internal class AssignStudentToClassCommandHandler : IRequestHandler<AssignStudentToClassCommand, Result>
{
    private const int MaxStudentsPerClass = 20;

    private readonly IClassRepository classRepository;

    public AssignStudentToClassCommandHandler(IClassRepository classRepository)
    {
        this.classRepository = classRepository;
    }

    public async Task<Result> Handle(
        AssignStudentToClassCommand request,
        CancellationToken cancellationToken)
    {
        var errors = await this.ValidateAsync(request, cancellationToken);
        if (errors.Count != 0)
        {
            var errorsDictionary = errors.ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, x => x.ToArray());
            return Result.Failure(ResultErrorType.Invalid, errorsDictionary);
        }

        await this.classRepository
            .AssignStudentAsync(request.ClassId, request.StudentId, cancellationToken);

        return Result.Success();
    }

    private async Task<IReadOnlyCollection<KeyValuePair<string, string>>> ValidateAsync(
        AssignStudentToClassCommand request,
        CancellationToken cancellationToken)
    {
        List<KeyValuePair<string, string>> errors = [];
        var classExists = await this.classRepository
            .ExistsAsync(request.ClassId, cancellationToken);
        if (!classExists)
        {
            var errorMessage = $"Class with id {request.ClassId} does not exist.";
            errors.Add(new(nameof(request.ClassId), errorMessage));
        }

        var studentCount = await this.classRepository
            .GetCountOfStudentsAsync(request.ClassId, cancellationToken);
        if (studentCount >= MaxStudentsPerClass)
        {
            var errorMessage = $"Class with id {request.ClassId} has reached maximum number of students.";
            errors.Add(new(nameof(request.ClassId), errorMessage));
        }

        var studentExists = await this.classRepository
            .StudentExistsAsync(request.StudentId, cancellationToken);;
        if (!studentExists)
        {
            var errorMessage = $"Student with id {request.StudentId} does not exist.";
            errors.Add(new(nameof(request.StudentId), errorMessage));
        }

        var isStudentInClass = await this.classRepository
            .IsStudentInClassAsync(request.StudentId, request.ClassId, cancellationToken);
        if (isStudentInClass)
        {
            var errorMessage = $"Student with id {request.StudentId} is already assigned to class {request.ClassId}.";
            errors.Add(new(nameof(request.StudentId), errorMessage));
        }
        return errors;
    }

}