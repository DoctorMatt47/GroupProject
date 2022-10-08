using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Complaints;

public interface IComplaintService
{
    Task<IEnumerable<ComplaintResponse>> GetByTopicId(Guid topicId, CancellationToken cancellationToken);
    Task<IdResponse<Guid>> CreateComplaint(ComplaintRequest request, CancellationToken cancellationToken);
}
