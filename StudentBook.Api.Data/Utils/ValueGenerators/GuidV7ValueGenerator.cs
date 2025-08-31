using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace StudentBook.Api.Data.Utils.ValueGenerators;

internal sealed class GuidV7ValueGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry)
    {
        return Guid.CreateVersion7();
    }

    public override bool GeneratesTemporaryValues => false;
}