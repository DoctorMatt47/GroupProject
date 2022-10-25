using GroupProject.Application.Common.Responses;

namespace GroupProject.Application.Sections;

public interface ISectionService
{
    Task<IEnumerable<SectionResponse>> Get(CancellationToken cancellationToken);
    Task<IdResponse<int>> Create(CreateSectionRequest request, CancellationToken cancellationToken);
}
