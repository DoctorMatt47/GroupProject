using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Sections;

public class SectionService : ISectionService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<SectionService> _logger;
    private readonly IMapper _mapper;

    public SectionService(IAppDbContext dbContext, IMapper mapper, ILogger<SectionService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SectionResponse>> Get(CancellationToken cancellationToken) =>
        await _dbContext.Set<Section>()
            .ProjectTo<SectionResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<IdResponse<int>> Create(CreateSectionRequest request, CancellationToken cancellationToken)
    {
        await _dbContext.Set<Section>().NoOneOrThrowAsync(
            s => s.Header == request.Header,
            $"header: {request.Header}",
            cancellationToken);

        var section = new Section(request.Header, request.Description);

        await _dbContext.Set<Section>().AddAsync(section, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created section with id: {Id}", section.Id);

        return new IdResponse<int>(section.Id);
    }

    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var section = await _dbContext.Set<Section>().FindOrThrowAsync(id, cancellationToken);
        _dbContext.Set<Section>().Remove(section);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(PutSectionRequest request, CancellationToken cancellationToken)
    {
        var section = await _dbContext.Set<Section>().FindOrThrowAsync(request.Id, cancellationToken);
        await _dbContext.Set<Section>().NoOneOrThrowAsync(
            s => s.Header == request.Header,
            $"header: {request.Header}",
            cancellationToken);

        section.Header = request.Header;
        section.Description = request.Description;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
