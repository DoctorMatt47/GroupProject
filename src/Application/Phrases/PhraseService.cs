using System.Text.RegularExpressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Phrases;

public class PhraseService : IPhraseService
{
    private readonly IAppDbContext _context;
    private readonly ILogger<PhraseService> _logger;
    private readonly IMapper _mapper;

    public PhraseService(IAppDbContext context, ILogger<PhraseService> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PhraseResponse>> GetForbidden(CancellationToken cancellationToken) =>
        await _context.Set<ForbiddenPhrase>()
            .ProjectTo<PhraseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<PhraseResponse>> GetVerificationRequired(CancellationToken cancellationToken) =>
        await _context.Set<VerificationRequiredPhrase>()
            .ProjectTo<PhraseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task UpdateForbidden(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken)
    {
        _context.Set<ForbiddenPhrase>().RemoveRange(_context.Set<ForbiddenPhrase>());

        var phrases = request.Select(p => new ForbiddenPhrase(p.Phrase));
        await _context.Set<ForbiddenPhrase>().AddRangeAsync(phrases, cancellationToken);

        _logger.LogInformation("Updated forbidden phrases");

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateVerificationRequired(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken)
    {
        _context.Set<VerificationRequiredPhrase>().RemoveRange(_context.Set<VerificationRequiredPhrase>());

        var phrases = request.Select(p => new VerificationRequiredPhrase(p.Phrase));
        await _context.Set<VerificationRequiredPhrase>().AddRangeAsync(phrases, cancellationToken);

        _logger.LogInformation("Updated verification required phrases");

        await _context.SaveChangesAsync(cancellationToken);
    }

    public bool ContainsPhrase(string text, string phrase) => new Regex($@"\W*((?i){phrase}(?-i))\W*").IsMatch(text);
}
