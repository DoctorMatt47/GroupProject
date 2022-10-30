using AutoMapper;
using GroupProject.Application.Common.Extensions;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Commentaries;

public class CommentaryMapping : Profile
{
    public CommentaryMapping()
    {
        CreateMap<Commentary, CommentaryResponse>()
            .MapRecordMember(c => c.UserLogin, c => c.User.Login);
    }
}
