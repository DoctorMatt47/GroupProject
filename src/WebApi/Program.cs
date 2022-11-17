using System.Reflection;
using GroupProject.Application.Common.Extensions;
using GroupProject.Infrastructure.Extensions;
using GroupProject.WebApi.Extensions;
using GroupProject.WebApi.Middlewares;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddApplication()
    .AddInfrastructure(connectionString)
    .AddBearerAuthentication(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddControllers();

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BanCheckAuthorizationMiddleware>();

var app = builder.Build();

app.Services.CallEntityInitializers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
