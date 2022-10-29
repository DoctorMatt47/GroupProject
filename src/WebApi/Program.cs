using System.Reflection;
using GroupProject.Application.Common.Extensions;
using GroupProject.Infrastructure.Extensions;
using GroupProject.Infrastructure.Persistence.Initializers;
using GroupProject.WebApi.Extensions;

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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializers = scope.ServiceProvider.GetServices<IEntityInitializer>();
    foreach (var initializer in initializers) initializer.Initialize();
}

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
