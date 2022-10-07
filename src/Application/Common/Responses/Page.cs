namespace GroupProject.Application.Common.Responses;

public record Page<T>(IEnumerable<T> List, int PageCount);
