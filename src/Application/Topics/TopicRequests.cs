using GroupProject.Application.Common.Requests;
using GroupProject.Domain.ValueObjects;

namespace GroupProject.Application.Topics;

public record CreateTopicRequest(
    string Header,
    string Description,
    CompileOptions? CompileOptions,
    Guid UserId,
    int SectionId);

public record GetTopicsRequest(
    PageRequest Page,
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
