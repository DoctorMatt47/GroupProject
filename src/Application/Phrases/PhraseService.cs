﻿using AutoMapper;
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
        await _context.Set<VerificationRequiredPhrase>()
            .ProjectTo<PhraseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<PhraseResponse>> GetVerificationRequired(CancellationToken cancellationToken) =>
        await _context.Set<VerificationRequiredPhrase>()
            .ProjectTo<PhraseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task PutForbidden(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken)
    {
        await _context.Database.ExecuteSqlRawAsync("delete from ForbiddenPhrases", cancellationToken);

        var phrases = request.Select(p => new ForbiddenPhrase(p.Phrase));
        await _context.Set<ForbiddenPhrase>().AddRangeAsync(phrases, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task PutVerificationRequired(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken)
    {
        await _context.Database.ExecuteSqlRawAsync("delete from VerificationRequiredPhrases", cancellationToken);

        var phrases = request.Select(p => new VerificationRequiredPhrase(p.Phrase));
        await _context.Set<VerificationRequiredPhrase>().AddRangeAsync(phrases, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
