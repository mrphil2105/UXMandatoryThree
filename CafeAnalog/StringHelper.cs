using System.Security.Cryptography;

namespace CafeAnalog;

public static class StringHelper
{
    public static string GenerateMail()
    {
        Span<byte> bytes = stackalloc byte[6];
        RandomNumberGenerator.Fill(bytes);

        return Convert.ToHexString(bytes)
            .ToLowerInvariant() + "@example.com";
    }
}
