using System;
using System.Security.Cryptography;
using System.Text;

namespace csod_edge_integrations_custom_provider_service
{
    public class CustomCrypto
    {
        private string _salt;
        public CustomCrypto(string salt)
        {
            _salt = salt;
        }

        public string GenerateSaltedHash(string password)
        {
            HashAlgorithm algorithm = SHA256.Create();

            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltInBytes = Encoding.UTF8.GetBytes(_salt);

            byte[] plainTextWithSaltBytes = new byte[passwordInBytes.Length + saltInBytes.Length];

            for (int i = 0; i < passwordInBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = passwordInBytes[i];
            }
            for (int i = 0; i < saltInBytes.Length; i++)
            {
                plainTextWithSaltBytes[passwordInBytes.Length + i] = saltInBytes[i];
            }

            var saltedPasswordHash = algorithm.ComputeHash(plainTextWithSaltBytes);

            return Convert.ToBase64String(saltedPasswordHash);
        }

        public bool DoPasswordsMatch(string targetPassword, string sourcePassword)
        {
            var hashedPassword = GenerateSaltedHash(targetPassword);

            byte[] inputPassowrdInBytes = Convert.FromBase64String(hashedPassword);
            byte[] passwordFromDbInBytes = Convert.FromBase64String(sourcePassword);

            if (inputPassowrdInBytes.Length != passwordFromDbInBytes.Length)
            {
                return false;
            }
            for (int i = 0; i < inputPassowrdInBytes.Length; i++)
            {
                if (inputPassowrdInBytes[i] != passwordFromDbInBytes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
