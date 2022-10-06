using System.Security.Cryptography;
using GroupProject.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GroupProject.Application.Common.Services;

public class PasswordHashService : IPasswordHashService
{
    private const int BitCount = 8;
    private const int IterationCount = 100_000;
    private const int SubkeyBitCount = 256;

    public byte[] GenerateSalt()
    {
        var salt = new byte[16];
        RandomNumberGenerator.Create().GetBytes(salt);
        return salt;
    }

    public byte[] Encode(string password, byte[] passwordSalt) =>
        KeyDerivation.Pbkdf2(
            password,
            passwordSalt,
            KeyDerivationPrf.HMACSHA256,
            IterationCount,
            SubkeyBitCount / BitCount);
}
