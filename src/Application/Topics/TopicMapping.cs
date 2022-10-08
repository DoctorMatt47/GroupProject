using AutoMapper;
using GroupProject.Application.Common.Extensions;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Topics;

public class TopicMapping : Profile
{
    public TopicMapping()
    {
        CreateMap<Topic, TopicResponse>();
        CreateMap<Topic, TopicByUserIdResponse>();
        CreateMap<Topic, TopicInfoForUserResponse>()
            .MapRecordMember(r => r.UserLogin, _ => string.Empty);

        CreateMap<Topic, TopicInfoForModeratorResponse>()
            .MapRecordMember(r => r.UserLogin, _ => string.Empty)
            .MapRecordMember(r => r.ComplaintCount, _ => 0);
    }
}
