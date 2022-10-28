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

        CreateMap<Topic, TopicInfoResponse>()
            .MapRecordMember(r => r.UserLogin, t => t.User.Login)
            .MapRecordMember(r => r.SectionHeader, t => t.Section.Header);
    }
}
