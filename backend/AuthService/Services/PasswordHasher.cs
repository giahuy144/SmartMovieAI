using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

    public bool Verify(string password, string passwordHash) =>
        CryptographicOperations.FixedTimeEquals(Convert.FromHexString(Hash(password)), Convert.FromHexString(passwordHash));
}
