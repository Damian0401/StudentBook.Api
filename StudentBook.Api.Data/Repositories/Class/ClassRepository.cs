using Microsoft.EntityFrameworkCore;
using StudentBook.Api.Data.Entities;

namespace StudentBook.Api.Data.Repositories.Class;

public interface IClassRepository
{
    Task CreateAsync(ClassEntity classEntity, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(Guid classId, CancellationToken cancellationToken = default);
    Task<ClassEntity?> GetByIdAsync(Guid classId, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClassEntity classEntity, CancellationToken cancellationToken = default);
    Task<long> GetOrCreateLeadingTeacherIdAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid classId, CancellationToken cancellationToken = default);
    Task<int> GetCountOfStudentsAsync(Guid classId, CancellationToken cancellationToken = default);
    Task<bool> IsStudentInClassAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default);
    Task<bool> StudentExistsAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task AssignStudentAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default);
    Task UnassignStudentAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default);
}

internal sealed class ClassRepository : IClassRepository
{
    private readonly StudentBookDb studentBookDb;

    public ClassRepository(StudentBookDb studentBookDb)
    {
        this.studentBookDb = studentBookDb;
    }

    public async Task CreateAsync(ClassEntity classEntity, CancellationToken cancellationToken = default)
    {
        this.studentBookDb.Classes.Add(classEntity);
        await this.studentBookDb.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        var deletedRows = await this.studentBookDb.Classes
            .Where(s => s.Id == classId)
            .ExecuteDeleteAsync(cancellationToken);
        return deletedRows > 0;
    }

    public async Task<ClassEntity?> GetByIdAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Classes
            .FirstOrDefaultAsync(s => s.Id == classId, cancellationToken);
    }

    public async Task UpdateAsync(ClassEntity classEntity, CancellationToken cancellationToken = default)
    {
        this.studentBookDb.Classes.Update(classEntity);
        await this.studentBookDb.SaveChangesAsync(cancellationToken);
    }

    public async Task<long> GetOrCreateLeadingTeacherIdAsync(string name, CancellationToken cancellationToken = default)
    {
        var leadingTeacher = await this.studentBookDb.LeadingTeachers
            .Where(x => x.Name == name)
            .Select(x => new {x.Id})
            .FirstOrDefaultAsync(cancellationToken);

        if (leadingTeacher is not null)
        {
            return leadingTeacher.Id;
        }

        var newLeadingTeacher = new LeadingTeacherEntity
        {
            Name = name,
        };
        this.studentBookDb.LeadingTeachers.Add(newLeadingTeacher);
        await this.studentBookDb.SaveChangesAsync(cancellationToken);
        return newLeadingTeacher.Id;
    }

    public async Task<bool> ExistsAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Classes
            .AnyAsync(s => s.Id == classId, cancellationToken);
    }

    public async Task<int> GetCountOfStudentsAsync(Guid classId, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Students
            .Where(s => s.ClassId == classId)
            .CountAsync(cancellationToken);
    }

    public async Task<bool> IsStudentInClassAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Students
            .AnyAsync(s => s.ClassId == classId && s.Id == studentId, cancellationToken);
    }

    public async Task<bool> StudentExistsAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Students
            .AnyAsync(s => s.Id == studentId, cancellationToken);
    }

    public async Task AssignStudentAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default)
    {
        await this.studentBookDb.Students
            .Where(s => s.Id == studentId)
            .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.ClassId, classId),
                cancellationToken);
    }

    public async Task UnassignStudentAsync(Guid classId, Guid studentId, CancellationToken cancellationToken = default)
    {
        await this.studentBookDb.Students
            .Where(s => s.Id == studentId)
            .ExecuteUpdateAsync(s =>
                    s.SetProperty(x => x.ClassId, (Guid?)null),
                cancellationToken);
    }
}