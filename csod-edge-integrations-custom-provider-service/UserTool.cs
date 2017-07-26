using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service
{
    public static class UserTool
    {
        private static readonly string _passwordSalt = "#*$#(fdadjfa1!jkdfahjfda";
        public static string GenerateSaltedHash(string password)
        {
            HashAlgorithm algorithm = SHA256.Create();

            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltInBytes = Encoding.UTF8.GetBytes(_passwordSalt);

            byte[] plainTextWithSaltBytes = new byte[passwordInBytes.Length + saltInBytes.Length];

            for(int i = 0; i < passwordInBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = passwordInBytes[i];
            }
            for(int i = 0; i < saltInBytes.Length; i++)
            {
                saltInBytes[passwordInBytes.Length + i] = saltInBytes[i];
            }

            var saltedPasswordHash = algorithm.ComputeHash(plainTextWithSaltBytes);

            return Convert.ToBase64String(saltedPasswordHash);
        }

        public static bool DoPasswordsMatch(string userInputPassword, string passwordFromDb)
        {
            var hashedPassword = GenerateSaltedHash(userInputPassword);

            byte[] inputPassowrdInBytes = Convert.FromBase64String(hashedPassword);
            byte[] passwordFromDbInBytes = Convert.FromBase64String(passwordFromDb);

            if(inputPassowrdInBytes.Length != passwordFromDbInBytes.Length)
            {
                return false;
            }
            for(int i = 0; i < inputPassowrdInBytes.Length; i++)
            {
                if(inputPassowrdInBytes[i] != inputPassowrdInBytes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
