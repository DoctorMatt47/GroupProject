using AutoMapper;
using GroupProject.Application.Topics;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Common.Mappings;

public class TopicMapping : Profile
{
    public TopicMapping()
    {
        CreateMap<Topic, TopicResponse>();
        CreateMap<Topic, TopicInfoForUserResponse>();
        CreateMap<Topic, TopicInfoForModeratorResponse>();
        CreateMap<Topic, TopicByUserIdResponse>();
    }
}
