using Asp.Versioning;

namespace StudentBook.Api.Constants;

internal static class Api
{
    internal static class Versions
    {
        internal static readonly ApiVersion V1 = new(1);
    }
    
    internal static class Tags
    {
        internal static readonly string Hello = nameof(Hello);
        internal static readonly string World = nameof(World);
    }
}