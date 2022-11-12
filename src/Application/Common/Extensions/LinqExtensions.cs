using System.Linq.Expressions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Application.Common.Extensions;

public static class LinqExtensions
{
    public static Page<T> ToPage<T>(this IEnumerable<T> enumerable, int pageCount) =>
        new(enumerable.ToList(), pageCount);

    public static async Task<TEntity> FirstOrThrowAsync<TEntity, TId>(
        this IQueryable<TEntity> set,
        TId id,
        CancellationToken cancellationToken)
        where TEntity : class, IHasId<TId>
        where TId : notnull
    {
        var lambda = EqualsLambdaExpression<TEntity>(id, "Id");
        var entity = await set.FirstOrDefaultAsync(lambda, cancellationToken);
        if (entity is null) throw new NotFoundException($"There is no {typeof(TEntity).Name} with id: {id}");
        return entity;
    }

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

    public static async Task NoOneOrThrowAsync<TEntity>(
        this IQueryable<TEntity> set,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var isExist = await set.AnyAsync(predicate, cancellationToken);
        if (isExist)
            throw new ConflictException($"There is already {typeof(TEntity).Name} with condition: {predicate}");
    }

    public static async Task AnyOrThrowAsync<TEntity, TId>(
        this IQueryable<TEntity> set,
        TId id,
        CancellationToken cancellationToken)
        where TEntity : class, IHasId<TId>
        where TId : notnull
    {
        var lambda = EqualsLambdaExpression<TEntity>(id, "Id");
        var isExist = await set.AnyAsync(lambda, cancellationToken);
        if (!isExist) throw new NotFoundException($"There is no {typeof(TEntity).Name} with id: {id}");
    }

    private static Expression<Func<TEntity, bool>> EqualsLambdaExpression<TEntity>(
        object? constant,
        string propertyName)
        where TEntity : class
    {
        var parameter = Expression.Parameter(typeof(TEntity), nameof(TEntity));
        var equal = Expression.Equal(Expression.Property(parameter, propertyName), Expression.Constant(constant));
        return Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
    }

    public static async Task<Page<T>> ToPageAsync<T>(
        this IQueryable<T> queryable,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        var (perPage, page) = request;

        var pageCount = await queryable.PageCountAsync(page, cancellationToken);
        var list = await queryable
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellationToken);

        return new Page<T>(list, pageCount);
    }

    private static async Task<int> PageCountAsync<T>(
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
}
