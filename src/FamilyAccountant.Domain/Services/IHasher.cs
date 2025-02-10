namespace FamilyAccountant.Domain.Services;

public interface IHasher
{
    (string Hash, string Salt) Hash(string input, string? salt = null);
    bool VerifyHash(string input, string salt, string hash);
}