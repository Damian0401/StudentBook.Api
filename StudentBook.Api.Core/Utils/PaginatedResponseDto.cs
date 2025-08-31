using Microsoft.EntityFrameworkCore;
using StudentBook.Api.Common.Constants;

namespace StudentBook.Api.Core.Utils;

internal static class PaginatedResponseDto
{
    public static async Task<PaginatedResponseDto<T>> FromAsync<T>(
        IQueryable<T> items,
        IPaginatedQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = query.Page ?? Pagination.DefaultPage;
        var size = query.Size ?? Pagination.DefaultSize;
        size = size > Pagination.MaxSize
            ? Pagination.MaxSize
            : size;

        var totalItems = await items.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        return new()
        {
            Items = await items.Skip(page * size).Take(size).ToListAsync(cancellationToken),
            PageNumber = page,
            PageSize = size,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}

public sealed record PaginatedResponseDto<T>
{
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
    public required int TotalItems { get; init; }
    public required int TotalPages { get; init; }
    public required IEnumerable<T> Items { get; init; }
}