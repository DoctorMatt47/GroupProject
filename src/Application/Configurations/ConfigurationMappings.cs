using AutoMapper;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Configurations;

public class ConfigurationMappings : Profile
{
    public ConfigurationMappings()
    {
        CreateMap<Configuration, ConfigurationResponse>();
    }
}
