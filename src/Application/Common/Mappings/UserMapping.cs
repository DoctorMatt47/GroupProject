using AutoMapper;
using GroupProject.Application.Users;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Common.Mappings;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserResponse>();
    }
}