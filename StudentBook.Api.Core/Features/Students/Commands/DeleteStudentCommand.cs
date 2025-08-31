using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Data.Repositories.Student;

namespace StudentBook.Api.Core.Features.Students.Commands;

public sealed record DeleteStudentCommand : IRequest<Result>
{
    public required Guid StudentId { get; init; }
}

internal class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Result>
{
    private readonly IStudentRepository studentRepository;

    public DeleteStudentCommandHandler(IStudentRepository studentRepository)
    {
        this.studentRepository = studentRepository;
    }

    public async Task<Result> Handle(
        DeleteStudentCommand request,
        CancellationToken cancellationToken)
    {
        var isDeleted = await this.studentRepository.DeleteByIdAsync(
            request.StudentId,
            cancellationToken);

        return isDeleted
            ? Result.Success()
            : Result.Failure(ResultErrorType.NotFound);
    }
}