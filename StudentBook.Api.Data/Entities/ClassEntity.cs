using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentBook.Api.Data.Utils.Abstraction;
using StudentBook.Api.Data.Utils.Constants;
using StudentBook.Api.Data.Utils.ValueConverters;

namespace StudentBook.Api.Data.Entities;

public sealed record ClassEntity : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public long LeadingTeacherId { get; set; }

    public LeadingTeacherEntity? LeadingTeacher { get; set; }
    public ICollection<StudentEntity>? Students { get; set; }

    internal class Configuration : BaseConfiguration<ClassEntity>
    {
        public override void Configure(EntityTypeBuilder<ClassEntity> builder)
        {
            // General
            base.Configure(builder);
            builder.ToTable("Classes", DatabaseSchemas.School);

            // Relations
            builder.HasOne(x => x.LeadingTeacher)
                .WithMany(x => x.Classes)
                .HasForeignKey(x => x.LeadingTeacherId);

            // Properties
            builder.Property(x => x.Id)
                .HasConversion(new GuidVersion7Converter());
        }
    }
}