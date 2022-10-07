using AutoMapper;
using GroupProject.Application.Topics;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Common.Mappings;

public class TopicMapping : Profile
{
    protected TopicMapping()
    {
        CreateMap<Topic, TopicResponse>();
        CreateMap<Topic, TopicInfoResponse>();
        CreateMap<Topic, TopicByUserIdResponse>();
    }
}
