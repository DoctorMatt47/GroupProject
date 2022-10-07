namespace GroupProject.Application.Users;

public interface IUserService
{
    Task<UserResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<AuthenticateUserResponse> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    Task<AuthenticateUserResponse> CreateModerator(CreateUserRequest request, CancellationToken cancellationToken);
    Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request, CancellationToken cancellationToken);
}
