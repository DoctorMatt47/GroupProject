using AutoMapper;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Complaints;
using GroupProject.Application.Sections;
using GroupProject.Application.Topics;
using GroupProject.Domain.Enums;
using GroupProject.Domain.ValueObjects;
using GroupProject.WebApi.Requests;

namespace GroupProject.WebApi.Mappings;

public class BodyToRequestMapping : Profile
{
    public BodyToRequestMapping()
    {
        CreateMap<CreateCommentaryBody, CreateCommentaryRequest>()
            .MapRecordMember(r => r.UserId, _ => Guid.Empty)
            .MapRecordMember(r => r.TopicId, _ => Guid.Empty);

        CreateMap<CreateComplaintBody, CreateComplaintRequest>()
            .MapRecordMember(r => r.UserId, _ => Guid.Empty)
            .MapRecordMember(r => r.Target, _ => ComplaintTarget.Topic)
            .MapRecordMember(r => r.TargetId, _ => Guid.Empty);

        CreateMap<CreateTopicBody, CreateTopicRequest>()
            .MapRecordMember(r => r.UserId, _ => Guid.Empty)
            .MapRecordMember(r => r.SectionId, _ => 0);

        CreateMap<CompileOptionsBody, CompileOptions>();

        CreateMap<PutSectionBody, PutSectionRequest>()
            .MapRecordMember(r => r.Id, _ => 0);
    }
}
