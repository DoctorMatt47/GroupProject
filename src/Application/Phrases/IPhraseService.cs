using System.Linq.Expressions;
using GroupProject.Domain.Entities;

namespace GroupProject.Application.Phrases;

public interface IPhraseService
{
    Task<IEnumerable<PhraseResponse>> GetForbidden(CancellationToken cancellationToken);
    Task<IEnumerable<PhraseResponse>> GetVerificationRequired(CancellationToken cancellationToken);

    Task UpdateForbidden(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken);

    Task UpdateVerificationRequired(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PhraseResponse>> GetForbiddenWhere(
        Expression<Func<ForbiddenPhrase, bool>> predicate,
        CancellationToken cancellationToken);
}
