using HackathonManager.ws.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginatedDto<T>> PaginateAsync<T>(this IQueryable<T> query, IPaginationFilter paginator)
        where T : class
        => new()
        {
            Items = await query.Skip((paginator.Page - 1) * paginator.PageSize).Take(paginator.PageSize).ToListAsync(),
            ItemsPerPage = paginator.PageSize,
            Page = paginator.Page,
            Total = await query.CountAsync(),
            TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)paginator.PageSize)
        };

    public static PaginatedDto<T> Paginate<T>(this IEnumerable<T> list, int page, IPaginationFilter paginator)
        where T : class
        => new()
        {
            Items = [.. list.Skip((page - 1) * paginator.PageSize).Take(paginator.PageSize)],
            ItemsPerPage = paginator.PageSize,
            Page = page,
            Total = list.Count(),
            TotalPages = (int)Math.Ceiling(list.Count() / (double)paginator.PageSize)
        };

    public static PaginatedDto<TDto> RePaginate<T, TDto>(this PaginatedDto<T> paginatedList, Func<T, TDto> selector)
        where T : class
        where TDto : class
        => new()
        {
            Items = paginatedList.Items.Select(selector),
            ItemsPerPage = paginatedList.ItemsPerPage,
            Page = paginatedList.Page,
            Total = paginatedList.Total,
            TotalPages = paginatedList.TotalPages
        };

    public static async Task<PaginatedDto<TDto>> RePaginateAsync<T, TDto>(this PaginatedDto<T> source, Func<T, Task<TDto>> mapper)
        where T : class
        where TDto : class
    {
        List<TDto> items = [];
        foreach (T item in source.Items!)
        {
            items.Add(await mapper(item));
        }

        return new PaginatedDto<TDto>
        {
            Items = items,
            Total = source.Total,
            Page = source.Page,
            ItemsPerPage = source.ItemsPerPage,
            TotalPages = source.TotalPages,
        };
    }
}
