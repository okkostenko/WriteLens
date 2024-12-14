using System.Security.Cryptography;
using System.Text;

namespace WriteLens.Shared.Utilities;

public static class HashUtility
{
    /// <summary>
    /// Generates a SHA256 hash for the given input string.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <returns>The SHA256 hash as a hexadecimal string.</returns>
    public static string GenerateHash(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));

        using (var sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // Convert to hexadecimal string
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2")); // Lowercase hexadecimal
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Validates whether the given input string matches the provided hash.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="hash">The hash to compare against.</param>
    /// <returns>True if the input string matches the hash; otherwise, false.</returns>
    public static bool ValidateHash(string input, string hash)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));

        if (string.IsNullOrEmpty(hash))
            throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));

        string generatedHash = GenerateHash(input);
        return string.Equals(generatedHash, hash, StringComparison.OrdinalIgnoreCase);
    }
}