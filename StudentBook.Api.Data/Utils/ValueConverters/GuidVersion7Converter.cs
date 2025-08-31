using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StudentBook.Api.Data.Utils.ValueConverters;

internal sealed class GuidVersion7Converter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<Guid, Guid>(guid => EnsureVersion7(guid), guid => guid, mappingHints)
{
    private static Guid EnsureVersion7(Guid guid)
    {
        return guid == Guid.Empty
            ? Guid.CreateVersion7()
            : guid;
    }
}