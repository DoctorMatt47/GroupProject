namespace GroupProject.Application.Users;

public record UserResponse(
    Guid Id,
    string Login,
    string Role,
    DateTime CreationTime,
    int WarningCount,
    DateTime BanEndTime);
