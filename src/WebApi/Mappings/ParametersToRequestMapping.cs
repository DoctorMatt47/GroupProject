using AutoMapper;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Topics;
using GroupProject.WebApi.Requests;

namespace GroupProject.WebApi.Mappings;

public class ParametersToRequestMapping : Profile
{
    public ParametersToRequestMapping()
    {
        CreateMap<GetTopicsParameters, GetTopicsRequest>();
        CreateMap<GetCommentariesParameters, GetCommentariesRequest>();
    }
}
