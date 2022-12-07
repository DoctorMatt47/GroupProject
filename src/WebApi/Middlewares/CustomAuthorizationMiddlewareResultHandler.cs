using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace GroupProject.WebApi.Middlewares;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _handler = new();
    private readonly IServiceProvider _provider;

    public CustomAuthorizationMiddlewareResultHandler(IServiceProvider provider) => _provider = provider;

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = 403;
            var failureReasons = authorizeResult.AuthorizationFailure?.FailureReasons.FirstOrDefault();
            if (failureReasons is not null) await context.Response.WriteAsJsonAsync(new {failureReasons.Message});
            return;
        }

        await _handler.HandleAsync(next, context, policy, authorizeResult);
    }
}
