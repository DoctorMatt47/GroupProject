using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.Sections;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.IntegrationTests.Sections;

public class SectionServiceTests
{
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;
    private readonly ISectionService _sections;

    public SectionServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        ISectionService sections)
    {
        _db = db;
        _dbContext = dbContext;
        _sections = sections;
    }

    [Fact]
    public async Task GetOrderedByCreationTime()
    {
        var random = new Random();
        var sectionCount = random.Next(10);
        for (var i = 0; i < sectionCount; i++)
        {
            var section = Section();
            _dbContext.Set<Section>().Add(section);
        }

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var sections = await _sections.Get(CancellationToken.None);
        sections.Should().HaveCountGreaterOrEqualTo(sectionCount);
    }

    [Fact]
    public async Task CreateSection()
    {
        var request = CreateSectionRequest();

        var response = await _sections.Create(request, CancellationToken.None);
        var section = await _dbContext.Set<Section>().FindAsync(response.Id);

        section.Should().NotBeNull();
        section!.Header.Should().Be(request.Header);
        section.Description.Should().Be(request.Description);
    }

    private static Section Section()
    {
        var fixture = new Fixture();
        return new Section(fixture.Create<string>(), fixture.Create<string>());
    }

    private static CreateSectionRequest CreateSectionRequest()
    {
        var fixture = new Fixture();
        return new CreateSectionRequest(fixture.Create<string>(), fixture.Create<string>());
    }
}
