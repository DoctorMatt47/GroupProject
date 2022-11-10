namespace GroupProject.Application.Phrases;

public interface IPhraseService
{
    Task<IEnumerable<PhraseResponse>> GetForbidden(CancellationToken cancellationToken);
    Task<IEnumerable<PhraseResponse>> GetVerificationRequired(CancellationToken cancellationToken);

    Task PutForbidden(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken);

    Task PutVerificationRequired(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken);
}
