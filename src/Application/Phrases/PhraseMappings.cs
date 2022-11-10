using AutoMapper;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Phrases;

public class PhraseMappings : Profile
{
    public PhraseMappings()
    {
        CreateMap<ForbiddenPhrase, PhraseResponse>();
        CreateMap<VerificationRequiredPhrase, PhraseResponse>();
    }
}
