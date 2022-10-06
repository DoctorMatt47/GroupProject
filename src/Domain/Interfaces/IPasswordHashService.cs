namespace GroupProject.Domain.Interfaces;

public interface IPasswordHashService
{
    byte[] GenerateSalt();
    byte[] Encode(string password, byte[] salt);
}