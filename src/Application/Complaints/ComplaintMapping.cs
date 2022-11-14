using AutoMapper;
using GroupProject.Application.Common.Extensions;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;

namespace GroupProject.Application.Complaints;

public class ComplaintMapping : Profile
{
    public ComplaintMapping()
    {
        CreateMap<Complaint, ComplaintByTargetResponse>();
        CreateMap<Complaint, ComplaintResponse>()
            .MapRecordMember(
                r => r.TargetId,
                c => c.Target == ComplaintTarget.Topic
                    ? c.TopicId
                    : c.CommentaryId);
    }
}
