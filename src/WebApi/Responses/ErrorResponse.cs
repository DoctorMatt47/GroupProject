namespace GroupProject.WebApi.Responses;

public record ErrorResponse(string? Message, string? HowToFix, string? HowToPrevent);

