using System.Reflection;
using GroupProject.Application.Common.Extensions;
using GroupProject.Infrastructure.Extensions;
using GroupProject.Infrastructure.Persistence.Initializers;
using GroupProject.WebApi.Extensions;
using GroupProject.WebApi.Middlewares;
using GroupProject.WebApi.Requirements;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var configurationSection = builder.Configuration.GetSection(nameof(ConfigurationOptions));
builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection(nameof(ConfigurationOptions)));
builder.Services.Configure<SectionOptions>(
    configurationSection.GetRequiredSection(nameof(ConfigurationOptions.Sections)));

builder.Services
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddApplication()
    .AddInfrastructure(connectionString)
    .AddBearerAuthentication(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>()
    .AddSingleton<IAuthorizationHandler, NotBannedHandler>()
    .AddAuthorization(opts => opts.AddPolicy("NotBanned", policy => policy.AddRequirements(new NotBannedRequirement())))
    .AddControllers();

var app = builder.Build();

app.Services.CallEntityInitializers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseFileServerWithoutCaching();
}
else
{
    app.UseFileServer();
}

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
