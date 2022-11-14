using GroupProject.Application.Common.Requests;

namespace GroupProject.WebApi.Requests;

public record GetTopicsParameters(
    PageRequest Page,
    string OrderBy,
    bool OnlyOpen,
    string? Substring,
    int? SectionId,
    Guid? UserId);
