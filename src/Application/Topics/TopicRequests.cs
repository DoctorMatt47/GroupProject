using GroupProject.Application.Common.Requests;

namespace GroupProject.Application.Topics;

public record CreateTopicRequest(
    string Header,
    string Description,
    CompileOptionsRequest? CompileOptions,
    Guid UserId,
    int SectionId);

public record GetTopicsRequest(
    string? Substring,
    int? SectionId,
    Guid? UserId,
    TopicsOrderedByParameter OrderBy,
    PageRequest PageRequest,
    bool IsVerificationRequired,
    bool IsOpen);

public enum TopicsOrderedByParameter
{
    CreationTime,
    ViewCount,
    ComplaintCount,
}
