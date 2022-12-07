using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.Phrases;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.IntegrationTests.Phrases;

public class PhraseServiceTests
{
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;
    private readonly IPhraseService _phrases;

    public PhraseServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        IPhraseService phrases)
    {
        _db = db;
        _dbContext = dbContext;
        _phrases = phrases;
    }

    [Fact]
    public async Task GetForbiddenPhrases()
    {
        var fixture = new Fixture();
        var phraseExpected = fixture.Create<string>();
        _dbContext.Set<ForbiddenPhrase>().Add(new ForbiddenPhrase(phraseExpected));
        await _dbContext.SaveChangesAsync();

        var forbidden = await _phrases.GetForbidden(CancellationToken.None);
        forbidden.Should().Contain(phrase => phrase.Phrase == phraseExpected);
    }

    [Fact]
    public async Task GetVerificationRequiredPhrases()
    {
        var fixture = new Fixture();
        var phraseExpected = fixture.Create<string>();
        _dbContext.Set<VerificationRequiredPhrase>().Add(new VerificationRequiredPhrase(phraseExpected));
        await _dbContext.SaveChangesAsync();

        var forbidden = await _phrases.GetVerificationRequired(CancellationToken.None);
        forbidden.Should().Contain(phrase => phrase.Phrase == phraseExpected);
    }
}
