using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GroupProject.WebApi.Requirements;

public record NotBannedRequirement : IAuthorizationRequirement;

public class NotBannedHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    private readonly IServiceProvider _provider;

    public NotBannedHandler(IServiceProvider provider) => _provider = provider;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IAuthorizationRequirement requirement)
    {
        await using var scope = _provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var userId = context.User.Identity?.Name!;
        var user = await dbContext.Set<User>().FindAsync(Guid.Parse(userId));

        if (user is null) return;

        if (User.IsBanned.Compile().Invoke(user))
        {
            var message = $@"{{""message"":""You have been banned until {user.BanEndTime}""}}";
            context.Fail(new AuthorizationFailureReason(this, message));
            return;
        }

        context.Succeed(requirement);
    }
}
