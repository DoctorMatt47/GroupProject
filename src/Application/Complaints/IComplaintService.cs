using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Complaints;

public interface IComplaintService
{
    Task<IEnumerable<ComplaintResponse>> GetByTopicId(Guid topicId, CancellationToken cancellationToken);
    Task<IEnumerable<ComplaintResponse>> GetByCommentaryId(Guid commentaryId, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> Create(CreateComplaintRequest request, CancellationToken cancellationToken);
}
