namespace IceVault.Common.Identity.Models.Results;

public class TokenResult
{
    public TokenResult(string accessToken, string refreshToken, int expiresIn)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
    }

    public string AccessToken { get; }

    public string RefreshToken { get; }

    public int ExpiresIn { get; }
}