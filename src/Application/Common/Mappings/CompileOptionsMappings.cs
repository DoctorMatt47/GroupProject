using AutoMapper;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.Common.Mappings;

public class CompileOptionsMappings : Profile
{
    public CompileOptionsMappings()
    {
        CreateMap<CompileOptions, CompileOptionsResponse>();
    }
}
