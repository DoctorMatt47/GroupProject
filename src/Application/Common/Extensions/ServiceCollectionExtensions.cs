using System.Reflection;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Common.Services;
using GroupProject.Application.Complaints;
using GroupProject.Application.Configurations;
using GroupProject.Application.Identity;
using GroupProject.Application.Phrases;
using GroupProject.Application.Sections;
using GroupProject.Application.Topics;
using GroupProject.Application.Users;
using GroupProject.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GroupProject.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddTransient<IPasswordHashService, PasswordHashService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICommentaryService, CommentaryService>();
        services.AddTransient<ITopicService, TopicService>();
        services.AddTransient<IComplaintService, ComplaintService>();
        services.AddTransient<ISectionService, SectionService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IConfigurationService, ConfigurationService>();
        services.AddTransient<IPhraseService, PhraseService>();

        return services;
    }
}
