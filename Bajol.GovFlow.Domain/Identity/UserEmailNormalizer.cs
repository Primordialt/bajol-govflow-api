namespace Bajol.GovFlow.Domain.Identity;

/// <summary>
/// Normalizes email addresses for case-insensitive uniqueness and lookup.
/// </summary>
public static class UserEmailNormalizer
{
    /// <summary>
    /// Returns a trimmed, invariant-lowercase email suitable for persistence and comparison.
    /// </summary>
    /// <param name="email">The raw email address.</param>
    /// <returns>The normalized email.</returns>
    public static string Normalize(string email) => email.Trim().ToLowerInvariant();
}
