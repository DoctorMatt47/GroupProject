namespace GroupProject.Application.Users;

public record AuthenticateUserResponse(
    string Token,
    Guid Id,
    string Role,
    string Status);
