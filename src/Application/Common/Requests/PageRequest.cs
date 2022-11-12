namespace GroupProject.Application.Common.Requests;

public record PageRequest(
    int PerPage,
    int Page);
