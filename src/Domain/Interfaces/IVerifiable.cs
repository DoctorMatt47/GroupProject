namespace GroupProject.Domain.Interfaces;

public interface IVerifiable
{
    DateTime? VerifyBefore { get; }
    void SetVerified();
}
