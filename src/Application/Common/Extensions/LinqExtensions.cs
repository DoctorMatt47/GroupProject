using GroupProject.Application.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Application.Common.Extensions;

public static class LinqExtensions
{
    public static Page<T> ToPage<T>(this IEnumerable<T> enumerable, int pageCount) =>
        new(enumerable.ToList(), pageCount);

    public static async Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> queryable,
        int pageCount,
        CancellationToken cancellationToken = default) =>
        new(await queryable.ToListAsync(cancellationToken), pageCount);

    public static async Task<int> PageCountAsync<T>(
        this IQueryable<T> queryable,
        int perPage,
        CancellationToken cancellationToken = default) =>
        (int) Math.Ceiling(await queryable.CountAsync(cancellationToken) / (float) perPage);

    public static async Task<IEnumerable<TResponse>> WhenAllAsync<TResponse>(this IEnumerable<Task<TResponse>> tasks)
    {
        var responses = new List<TResponse>();
        foreach (var task in tasks)
        {
            var response = await task;
            responses.Add(response);
        }

        return responses;
    }
}
