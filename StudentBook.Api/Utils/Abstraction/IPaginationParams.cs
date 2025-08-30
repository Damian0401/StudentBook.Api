namespace StudentBook.Api.Utils.Abstraction;

internal interface IPaginationParams
{
    public int? Page { get; init; }
    public int? Size { get; init; }
}