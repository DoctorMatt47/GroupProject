using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GroupProject.WebApi.Requirements;

public record NotBannedRequirement : IAuthorizationRequirement;

public class NotBannedHandler : AuthorizationHandler<NotBannedRequirement>
{
    private readonly IServiceProvider _provider;

    public NotBannedHandler(IServiceProvider provider) => _provider = provider;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        NotBannedRequirement requirement)
    {
        var userId = context.User.Identity?.Name;
        if (userId is null) return;

        await using var scope = _provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var user = await dbContext.Set<User>().FindAsync(Guid.Parse(userId));
        if (user is null) return;

        if (User.IsBanned.Compile().Invoke(user))
        {
            var message = $"You have been banned until {user.BanEndTime} utc";
            context.Fail(new AuthorizationFailureReason(this, message));
            return;
        }

        context.Succeed(requirement);
    }
}
