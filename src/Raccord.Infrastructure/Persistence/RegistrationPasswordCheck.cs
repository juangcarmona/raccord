using System.Security.Cryptography;
using System.Text;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Checks public password ranges without transmitting the password or full hash.</summary>
public sealed class RegistrationPasswordCheck(HttpClient client)
{
    /// <summary>Returns clear, matched or unavailable; a warning never prevents registration.</summary>
    public async Task<string> CheckAsync(string password, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(password);
        // SHA-1 is the public range protocol, never the account password-storage algorithm.
#pragma warning disable CA5350
        var bytes = Encoding.UTF8.GetBytes(password);
        string hash;
        try
        {
            hash = Convert.ToHexString(SHA1.HashData(bytes));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(bytes);
        }
#pragma warning restore CA5350
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("https://api.pwnedpasswords.com/range/" + hash[..5])
            );
            request.Headers.Add("Add-Padding", "true");
            using var response = await client
                .SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return "unavailable";
            var content = await response
                .Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);
            return CompareRange(content, hash[5..]);
        }
        catch (HttpRequestException)
        {
            return "unavailable";
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return "unavailable";
        }
    }

    private static string CompareRange(string content, string suffix)
    {
        var valid = false;
        foreach (var line in content.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Trim().Split(':');
            if (
                parts.Length != 2
                || parts[0].Length != 35
                || !long.TryParse(
                    parts[1],
                    System.Globalization.NumberStyles.None,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var count
                )
            )
                continue;
            valid = true;
            if (count > 0 && string.Equals(parts[0], suffix, StringComparison.OrdinalIgnoreCase))
                return "matched";
        }
        return valid ? "clear" : "unavailable";
    }
}
