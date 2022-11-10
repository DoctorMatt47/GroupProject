﻿using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Complaints;

public interface IComplaintService
{
    Task<Page<ComplaintResponse>> Get(int perPage, int page, CancellationToken cancellationToken);
    Task<IEnumerable<ComplaintByTargetResponse>> GetByTopicId(Guid topicId, CancellationToken cancellationToken);

    Task<IEnumerable<ComplaintByTargetResponse>> GetByCommentaryId(
        Guid commentaryId,
        CancellationToken cancellationToken);

    Task<IdResponse<Guid>> Create(CreateComplaintRequest request, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}
