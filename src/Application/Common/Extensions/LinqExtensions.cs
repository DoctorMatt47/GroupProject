using System.Linq.Expressions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Application.Common.Extensions;

public static class LinqExtensions
{
    public static Page<T> ToPage<T>(this IEnumerable<T> enumerable, int pageCount) =>
        new(enumerable.ToList(), pageCount);

    public static async Task<TEntity> FindOrThrowAsync<TEntity>(
        this DbSet<TEntity> set,
        object id,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var entity = await set.FindAsync(new[] {id}, cancellationToken);
        if (entity is null) throw new NotFoundException($"There is no {typeof(TEntity).Name} with id: {id}");
        return entity;
    }

    public static async Task AssertAnyAsync<TEntity>(
        this DbSet<TEntity> set,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        where TEntity : class =>
        throw new NotImplementedException();

    public static async Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> queryable,
        int perPage,
        int page,
        int pageCount,
        CancellationToken cancellationToken = default)
    {
        var list = await queryable
            .Skip((page - 1) * perPage)
            .Take(page)
            .ToListAsync(cancellationToken);

        return new Page<T>(list, pageCount);
    }

    public static async Task<int> PageCountAsync<T>(
        this IQueryable<T> queryable,
        int perPage,
        CancellationToken cancellationToken = default) =>
        (int) Math.Ceiling(await queryable.CountAsync(cancellationToken) / (float) perPage);

    public static async Task<IEnumerable<TResponse>> SelectAsync<TEntity, TResponse>(
        this IEnumerable<TEntity> enumerable,
        Func<TEntity, Task<TResponse>> selectTask)
    {
        var tasks = enumerable.Select(selectTask);

        var responses = new List<TResponse>();
        foreach (var task in tasks)
        {
            var response = await task;
            responses.Add(response);
        }

        return responses;
    }

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
