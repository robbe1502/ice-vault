using IceVault.Common.Identity.Models.Results;

namespace IceVault.Common.Identity;

public interface IIdentityProvider
{
    Task<TokenResult> GetTokenAsync(string email, string password);

    Task<TokenResult> RefreshTokenAsync(string token);

    Task<UserResult> GetProfileAsync(string token);

    Task<Guid> RegisterUserAsync(string firstName, string lastName, string email, string password, string locale, string timeZone, string currency);
}