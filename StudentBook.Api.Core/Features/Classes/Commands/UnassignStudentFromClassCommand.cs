using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Data.Repositories.Class;

namespace StudentBook.Api.Core.Features.Classes.Commands;

public sealed record UnassignStudentFromClassCommand : IRequest<Result>
{
    public required Guid ClassId { get; init; }

    public required Guid StudentId { get; init; }
}

internal class UnassignStudentFromClassCommandHandler : IRequestHandler<UnassignStudentFromClassCommand, Result>
{
    private readonly IClassRepository classRepository;

    public UnassignStudentFromClassCommandHandler(IClassRepository classRepository)
    {
        this.classRepository = classRepository;
    }

    public async Task<Result> Handle(
        UnassignStudentFromClassCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await this.ValidateAsync(request, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        await this.classRepository
            .UnassignStudentAsync(request.ClassId, request.StudentId, cancellationToken);

        return Result.Success();
    }

    private async Task<Result> ValidateAsync(
        UnassignStudentFromClassCommand request,
        CancellationToken cancellationToken)
    {
        var classExists = await this.classRepository.ExistsAsync(
            request.ClassId,
            cancellationToken);

        if (!classExists)
        {
            return Result.Failure(ResultErrorType.NotFound);
        }

        var studentExists = await this.classRepository
            .StudentExistsAsync(request.StudentId,
                cancellationToken);
        if (!studentExists)
        {
            return Result.Failure(ResultErrorType.NotFound);
        }

        var isStudentInClass = await this.classRepository
            .IsStudentInClassAsync(
                request.ClassId,
                request.StudentId,
                cancellationToken);

        if (!isStudentInClass)
        {
            var errors = new Dictionary<string, string[]>
            {
                {
                    nameof(request.StudentId),
                    [$"Student with id {request.StudentId} is not assigned to class {request.ClassId}."]
                },
            };
            return Result.Failure(ResultErrorType.Invalid, errors);
        }

        return Result.Success();
    }
}