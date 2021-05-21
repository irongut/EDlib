using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EDlib.Network
{
    /// <summary>A helper class for generating an SHA256 hash.</summary>
    public static class Sha256Helper
    {
        /// <summary>Generates the SHA256 hash of a string.</summary>
        /// <param name="input">The string to be hashed.</param>
        /// <returns>string</returns>
        public static string GenerateHash(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                return BytesToHexString(sha.ComputeHash(bytes, 0, bytes.Length));
            }
        }

        private static string BytesToHexString(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }
    }
}
