using AutoMapper;
using GroupProject.Application.Common.Requests;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Identity;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<User, CreateIdentityRequest>();
    }
}
