using System;
using System.Security.Cryptography;
using System.Text;

namespace DemoPick.Services
{
    internal static class AuthPasswordCrypto
    {
        internal static byte[] GenerateSalt(int size)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        internal static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password ?? string.Empty, salt, 100_000))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        internal static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        internal static string GenerateRandomPassword(int length)
        {
            const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%";
            if (length < 8) length = 8;

            byte[] bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(alphabet[bytes[i] % alphabet.Length]);
            }
            return sb.ToString();
        }
    }
}
