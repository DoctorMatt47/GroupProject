namespace GroupProject.Application.Users;

public record UserResponse(
    string Login,
    string Role,
    DateTime CreationTime);
