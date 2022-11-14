using GroupProject.Domain.Enums;

namespace GroupProject.Application.Identity;

public record Identity(
    Guid Id,
    UserRole Role);
