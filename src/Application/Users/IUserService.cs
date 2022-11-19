using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Users;

public interface IUserService
{
    Task<Page<UserResponse>> GetModerators(PageRequest request, CancellationToken cancellationToken);
    Task<Page<UserResponse>> GetUsers(PageRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<UserResponse>> GetBannedUsers(CancellationToken cancellationToken);
    Task<UserResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> CreateModerator(CreateUserRequest request, CancellationToken cancellationToken);
    Task AddWarningToUser(Guid id, CancellationToken cancellationToken);
    Task BanUser(Guid id, CancellationToken cancellationToken);
    Task UnbanUser(Guid id, CancellationToken cancellationToken);
}
