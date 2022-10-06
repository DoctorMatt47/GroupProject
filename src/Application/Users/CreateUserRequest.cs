namespace GroupProject.Application.Users;

public record CreateUserRequest(
    string Login,
    string Password);
