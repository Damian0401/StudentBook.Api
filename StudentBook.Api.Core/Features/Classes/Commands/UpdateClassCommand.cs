using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Core.Features.Classes.Models;
using StudentBook.Api.Data.Repositories.Class;

namespace StudentBook.Api.Core.Features.Classes.Commands;

public sealed class UpdateClassCommand : IRequest<Result>
{
    public required Guid ClassId { get; init; }

    public required UpdateClassDto Class { get; init; }
}

internal class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, Result>
{
    private readonly IClassRepository classRepository;

    public UpdateClassCommandHandler(IClassRepository classRepository)
    {
        this.classRepository = classRepository;
    }

    public async Task<Result> Handle(
        UpdateClassCommand request,
        CancellationToken cancellationToken)
    {
        var classToUpdate = await this.classRepository.GetByIdAsync(request.ClassId, cancellationToken);

        if (classToUpdate is null)
        {
            return Result.Failure(ResultErrorType.NotFound);
        }

        classToUpdate.Name = request.Class.Name;
        classToUpdate.LeadingTeacherId = await this.classRepository.GetOrCreateLeadingTeacherIdAsync(
            request.Class.LeadingTeacher,
            cancellationToken);

        await this.classRepository.UpdateAsync(classToUpdate, cancellationToken);
        return Result.Success();
    }
}