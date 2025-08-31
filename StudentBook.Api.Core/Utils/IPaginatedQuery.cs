namespace StudentBook.Api.Core.Utils;

public interface IPaginatedQuery
{
    public int? Page { get; init; }
    public int? Size { get; init; }
}