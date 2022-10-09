using AutoMapper;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Commentaries;

public class CommentaryMapping : Profile
{
    public CommentaryMapping()
    {
        CreateMap<Commentary, CommentaryResponse>();
    }
}
