using AutoMapper;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Complaints;
using GroupProject.Application.Topics;
using GroupProject.WebApi.Requests;

namespace GroupProject.WebApi.Mappings;

public class BodyToRequestMapping : Profile
{
    protected BodyToRequestMapping()
    {
        CreateMap<CreateCommentaryBody, CreateCommentaryRequest>();
        CreateMap<CreateComplaintBody, CreateComplaintRequest>();
        CreateMap<CreateTopicBody, CreateTopicRequest>();
    }
}
