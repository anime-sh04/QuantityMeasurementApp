using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public interface IEncryptionService
    {
        string HashPassword(string password);
        bool   VerifyPassword(string password, string hash);
    }

    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = ComputeSaltedHash(password, salt);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;
            var salt         = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);
            var actualHash   = ComputeSaltedHash(password, salt);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        private static byte[] ComputeSaltedHash(string password, byte[] salt)
        {
            var combined = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            return SHA256.HashData(combined);
        }
    }
}
