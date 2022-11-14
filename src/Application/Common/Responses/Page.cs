namespace GroupProject.Application.Common.Responses;

public record Page<T>(
    IEnumerable<T> Items,
    int PageCount,
    int ItemsCount);
