using Microsoft.EntityFrameworkCore;
using StudentBook.Api.Data.Entities;

namespace StudentBook.Api.Data.Repositories.Student;

public interface IStudentRepository
{
    Task<bool> IdentifierExistsAsync(string identifier, CancellationToken cancellationToken = default);
    Task CreateAsync(StudentEntity student, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<StudentEntity?> GetByIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task UpdateAsync(StudentEntity student, CancellationToken cancellationToken = default);
}

internal sealed class StudentRepository : IStudentRepository
{
    private readonly StudentBookDb studentBookDb;

    public StudentRepository(StudentBookDb studentBookDb)
    {
        this.studentBookDb = studentBookDb;
    }
    public async Task<bool> IdentifierExistsAsync(string identifier, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Students
            .AnyAsync(s =>
                s.Identifier == identifier,
                cancellationToken);
    }

    public async Task CreateAsync(StudentEntity student, CancellationToken cancellationToken = default)
    {
        this.studentBookDb.Students.Add(student);
        await this.studentBookDb.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var deletedRows = await this.studentBookDb.Students
            .Where(s => s.Id == studentId)
            .ExecuteDeleteAsync(cancellationToken);
        return deletedRows > 0;
    }

    public async Task<StudentEntity?> GetByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await this.studentBookDb.Students
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);
    }

    public async Task UpdateAsync(StudentEntity student, CancellationToken cancellationToken = default)
    {
        this.studentBookDb.Students.Update(student);
        await this.studentBookDb.SaveChangesAsync(cancellationToken);
    }
}