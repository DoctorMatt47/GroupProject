using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Users;

public interface IUserService
{
    Task<UserResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> CreateModerator(CreateUserRequest request, CancellationToken cancellationToken);
    Task AddWarningToUser(Guid id, CancellationToken cancellationToken);
}
