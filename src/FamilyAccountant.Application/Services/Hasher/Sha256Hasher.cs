using System.Security.Cryptography;
using System.Text;
using FamilyAccountant.Domain.Services;

namespace FamilyAccountant.Application.Services.Hasher;

public class Sha256Hasher : IHasher
{
    public (string Hash, string Salt) Hash(string input, string? salt = null)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(input);
        var saltBytes = GenerateSalt(salt);

        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(saltBytes.Concat(passwordBytes).ToArray());

        return (Convert.ToBase64String(hashedBytes), Convert.ToBase64String(saltBytes));
    }

    public bool VerifyHash(string input, string salt, string hash) => Hash(input, salt).Hash == hash;

    private static byte[] GenerateSalt(string? salt)
    {
        if (!string.IsNullOrWhiteSpace(salt))
            return Convert.FromBase64String(salt);
        
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        
        return saltBytes;
    }
}