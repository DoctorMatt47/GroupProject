using System.Text;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace GroupProject.WebApi.Middlewares;

public class BanCheckAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _handler = new();
    private readonly IServiceProvider _provider;

    public BanCheckAuthorizationMiddleware(IServiceProvider provider) => _provider = provider;

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Succeeded) await _handler.HandleAsync(next, context, policy, authorizeResult);

        await using var scope = _provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var userId = context.User.Identity?.Name!;
        var user = await dbContext.Set<User>().FindAsync(
            new object[] {Guid.Parse(userId)},
            context.RequestAborted);

        if (user is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (User.IsBanned.Compile().Invoke(user))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.Headers.ContentType = "application/json";
            context.Response.Headers.AcceptCharset = "utf-8";
            var bytes = Encoding.UTF8.GetBytes($@"{{""message"":""You have been banned until {user.BanEndTime}""}}");
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length, context.RequestAborted);
            return;
        }

        await _handler.HandleAsync(next, context, policy, authorizeResult);
    }
}
