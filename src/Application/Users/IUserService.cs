namespace GroupProject.Application.Users;

public interface IUserService
{
    Task<AuthenticateUserResponse> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);

    Task<AuthenticateUserResponse> CreateModerator(CreateUserRequest request, CancellationToken cancellationToken);

    Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request, CancellationToken cancellationToken);
}
