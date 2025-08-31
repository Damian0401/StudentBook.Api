using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentBook.Api.Data.Utils.Abstraction;
using StudentBook.Api.Data.Utils.Constants;

namespace StudentBook.Api.Data.Entities;

public sealed record LeadingTeacherEntity : BaseEntity<long>
{
    public required string Name { get; set; }

    public ICollection<ClassEntity>? Classes { get; set; }

    internal class Configuration : BaseConfiguration<LeadingTeacherEntity>
    {
        public override void Configure(EntityTypeBuilder<LeadingTeacherEntity> builder)
        {
            // General
            base.Configure(builder);
            builder.ToTable("LeadingTeachers", DatabaseSchemas.School);

            // Properties
            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}