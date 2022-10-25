using AutoMapper;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Sections;

public class SectionMapper : Profile
{
    public SectionMapper()
    {
        CreateMap<Section, SectionResponse>();
    }
}
