using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentBook.Api.Data.Utils.Abstraction;
using StudentBook.Api.Data.Utils.Constants;
using StudentBook.Api.Data.Utils.ValueConverters;

namespace StudentBook.Api.Data.Entities;

public sealed record StudentEntity : BaseEntity<Guid>
{
    public required string Identifier { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public Guid? ClassId { get; set; }

    public ClassEntity? Class { get; set; }

    internal class Configuration : BaseConfiguration<StudentEntity>
    {
        public override void Configure(EntityTypeBuilder<StudentEntity> builder)
        {
            // General
            base.Configure(builder);
            builder.ToTable("Students", DatabaseSchemas.School);

            // Relations
            builder.HasOne(x => x.Class)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.ClassId);

            // Properties
            builder.Property(x => x.Id)
                .HasConversion(new GuidVersion7Converter());
            builder.HasIndex(x => x.Identifier)
                .IsUnique();
        }
    }
}