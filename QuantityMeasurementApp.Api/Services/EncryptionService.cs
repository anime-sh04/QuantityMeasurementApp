using System.Security.Cryptography;
using System.Text;

namespace QuantityMeasurementApp.Api.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherTextBase64);

        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(IConfiguration config)
        {
            var raw = config["Encryption:Key"]
                      ?? throw new InvalidOperationException("Encryption:Key is not configured.");
            _key = SHA256.HashData(Encoding.UTF8.GetBytes(raw)); // always 32 bytes
        }


        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key  = _key;
            aes.Mode = CipherMode.CBC;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes  = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV,         0, result, 0,            aes.IV.Length);
            Buffer.BlockCopy(cipherBytes,    0, result, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherTextBase64)
        {
            var allBytes = Convert.FromBase64String(cipherTextBase64);

            using var aes = Aes.Create();
            aes.Key  = _key;
            aes.Mode = CipherMode.CBC;

            // First 16 bytes are the IV.
            var iv          = allBytes[..16];
            var cipherBytes = allBytes[16..];
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        // Password hashing
        public string HashPassword(string password)
        {
            // Generate a random 16-byte salt per password.
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = ComputeSaltedHash(password, salt);

            // Store as "base64(salt):base64(hash)"
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
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
