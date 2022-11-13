using GroupProject.Application.Common.Requests;

namespace GroupProject.Application.Topics;

public record CreateTopicRequest(
    string Header,
    string Description,
    CompileOptionsRequest? CompileOptions,
    Guid UserId,
    int SectionId);

public record GetTopicsRequest(
    PageRequest PageRequest,
    TopicsOrderedBy OrderBy,
    bool OnlyOpen = false,
    string? Substring = null,
    int? SectionId = null,
    Guid? UserId = null);

public enum TopicsOrderedBy
{
    CreationTime,
    ViewCount,
    ComplaintCount,
    VerifyBefore,
}
