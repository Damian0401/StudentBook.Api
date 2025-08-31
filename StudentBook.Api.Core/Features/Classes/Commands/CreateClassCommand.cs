using MediatR;
using StudentBook.Api.Common.Shared;
using StudentBook.Api.Core.Features.Classes.Models;
using StudentBook.Api.Data.Entities;
using StudentBook.Api.Data.Repositories.Class;

namespace StudentBook.Api.Core.Features.Classes.Commands;

public sealed record CreateClassCommand : IRequest<Result>
{
    public required CreateClassDto Class { get; init; }
}

internal class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Result>
{
    private readonly IClassRepository classRepository;

    public CreateClassCommandHandler(IClassRepository classRepository)
    {
        this.classRepository = classRepository;
    }

    public async Task<Result> Handle(
        CreateClassCommand request,
        CancellationToken cancellationToken)
    {
        var leadingTeacherId = await this.classRepository.GetOrCreateLeadingTeacherIdAsync(
            request.Class.LeadingTeacher,
            cancellationToken);

        var classEntity = new ClassEntity
        {
            Name = request.Class.Name,
            LeadingTeacherId = leadingTeacherId,
        };

        await this.classRepository.CreateAsync(classEntity, cancellationToken);
        return Result.Success();
    }
}