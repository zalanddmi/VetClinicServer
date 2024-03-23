using System.Security.Cryptography;

namespace VetClinicServer.Utils
{
    public class PasswordHasher
    {
        private const int SALT_SIZE = 128 / 8;
        private const int KEY_SIZE = 256 / 8;
        private const int ITERATIONS = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA3_256;

        public (string, string) Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, _hashAlgorithmName, KEY_SIZE);
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public bool Verify(string passwordHash, string saltHash, string inputPassword)
        {
            byte[] salt = Convert.FromBase64String(saltHash);
            byte[] hash = Convert.FromBase64String(passwordHash);

            byte[] hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, ITERATIONS, _hashAlgorithmName, KEY_SIZE);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }
    }
}
