using AutoMapper;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Users;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserResponse>();
    }
}
