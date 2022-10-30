using GroupProject.Domain.Enums;

namespace GroupProject.Application.Common.Requests;

public record CreateIdentityRequest(
    Guid Id,
    UserRole Role);
