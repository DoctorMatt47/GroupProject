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

    bool ContainsPhrase(string text, string phrase);
}
