namespace IceVault.Common.Identity;

public interface IIdentityProvider
{
    Task<TokenResult> GetTokenAsync(string email, string password);
}