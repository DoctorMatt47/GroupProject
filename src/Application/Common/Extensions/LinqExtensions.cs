using GroupProject.Application.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Application.Common.Extensions;

public static class LinqExtensions
{
    public static async Task<Page<T>> ToPageAsync<T>(this IQueryable<T> queryable, int pageCount) =>
        new(await queryable.ToListAsync(), pageCount);

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
