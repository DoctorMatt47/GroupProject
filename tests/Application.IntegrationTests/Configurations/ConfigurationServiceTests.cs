using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Configurations;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Application.IntegrationTests.Configurations;

public class ConfigurationServiceTests
{
    private readonly IConfigurationService _configuration;
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;

    public ConfigurationServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        IConfigurationService configuration)
    {
        _db = db;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [Fact]
    public async Task GetConfiguration()
    {
        var configuration = await _dbContext.Set<Configuration>().FirstAsync();

        var fixture = new Fixture();

        var rules = fixture.Create<string>();
        var warningCountForBan = fixture.Create<int>();
        var banDuration = fixture.Create<TimeSpan>();
        var complainDuration = fixture.Create<TimeSpan>();

        configuration.Rules = rules;
        configuration.BanDuration = banDuration;
        configuration.WarningCountForBan = warningCountForBan;
        configuration.ComplaintDuration = complainDuration;

        await _dbContext.SaveChangesAsync();

        var response = await _configuration.Get(CancellationToken.None);
        response.Rules.Should().Be(rules);
        response.BanDuration.Should().Be(banDuration);
        response.WarningCountForBan.Should().Be(warningCountForBan);
        response.ComplaintDuration.Should().Be(complainDuration);
    }

    [Fact]
    public async Task PatchConfiguration()
    {
        var fixture = new Fixture();

        var request = fixture.Create<PatchConfigurationRequest>();
        await _configuration.Patch(request, CancellationToken.None);

        var configuration = await _dbContext.Set<Configuration>().FirstAsync();

        configuration.Rules.Should().Be(request.Rules);
        configuration.WarningCountForBan.Should().Be(request.WarningCountForBan);
        configuration.BanDuration.Should().Be(request.BanDuration!.Value);
        configuration.ComplaintDuration.Should().Be(request.ComplaintDuration!.Value);
    }
}
