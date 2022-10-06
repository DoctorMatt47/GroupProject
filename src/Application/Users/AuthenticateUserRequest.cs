namespace GroupProject.Application.Users;

public record AuthenticateUserRequest(
    string Login,
    string Password);
