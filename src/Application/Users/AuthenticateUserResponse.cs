namespace GroupProject.Application.Users;

public record AuthenticateUserResponse(
    string Token,
    string Role,
    string Status);
