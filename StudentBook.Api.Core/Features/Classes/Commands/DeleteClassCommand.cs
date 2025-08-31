using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Data.Repositories.Class;

namespace StudentBook.Api.Core.Features.Classes.Commands;

public sealed record DeleteClassCommand : IRequest<Result>
{
    public required Guid ClassId { get; init; }
}

internal class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, Result>
{
    private readonly IClassRepository classRepository;

    public DeleteClassCommandHandler(IClassRepository classRepository)
    {
        this.classRepository = classRepository;
    }

    public async Task<Result> Handle(
        DeleteClassCommand request,
        CancellationToken cancellationToken)
    {
        var isDeleted = await this.classRepository.DeleteByIdAsync(
            request.ClassId,
            cancellationToken);

        return isDeleted
            ? Result.Success()
            : Result.Failure(ResultErrorType.NotFound);
    }
}