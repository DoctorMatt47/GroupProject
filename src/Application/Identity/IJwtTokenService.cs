using GroupProject.Domain.Enums;

namespace GroupProject.Application.Identity;

public interface IJwtTokenService
{
    string Get(Guid userId, UserRole role);
}