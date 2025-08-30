using Asp.Versioning;

namespace StudentBook.Api.Utils.Constants;

internal static class Api
{
    internal static class Versions
    {
        internal static readonly ApiVersion V1 = new(1);
    }

    internal static class Tags
    {
        internal const string Students = nameof(Students);
        internal const string Classes = nameof(Classes);
    }
}