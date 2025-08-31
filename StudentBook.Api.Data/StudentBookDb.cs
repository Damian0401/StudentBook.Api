using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudentBook.Api.Data.Entities;
using StudentBook.Api.Data.Utils.Abstraction;

namespace StudentBook.Api.Data;

public sealed class StudentBookDb : DbContext
{
    public StudentBookDb(
        DbContextOptions<StudentBookDb> options,
        TimeProvider timeProvider)
        : base(options)
    {
        this.ChangeTracker.Tracked += (_, args) => HandleStateChange(args, timeProvider);
        this.ChangeTracker.StateChanged += (_, args) => HandleStateChange(args, timeProvider);
    }

    // Entities
    public DbSet<ClassEntity> Classes => this.Set<ClassEntity>();
    public DbSet<StudentEntity> Students => this.Set<StudentEntity>();
    public DbSet<LeadingTeacherEntity> LeadingTeachers => this.Set<LeadingTeacherEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentBookDb).Assembly);
    }

    private static void HandleStateChange(
        EntityEntryEventArgs args,
        TimeProvider timeProvider)
    {
        if (args.Entry.Entity is not ITimeTrackable timeTrackable)
        {
            return;
        }

        switch (args.Entry.State)
        {
            case EntityState.Added:
                timeTrackable.CreatedAt = timeProvider.GetUtcNow();
                timeTrackable.UpdatedAt = timeProvider.GetUtcNow();
                break;
            case EntityState.Modified:
                timeTrackable.UpdatedAt = timeProvider.GetUtcNow();
                break;
        }
    }
}