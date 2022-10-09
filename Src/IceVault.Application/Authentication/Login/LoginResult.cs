namespace IceVault.Application.Authentication.Login;

public class LoginResult
{
    public LoginResult(string accessToken, string refreshToken, int expiresIn)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
    }

    public string AccessToken { get; }

    public string RefreshToken { get; }

    public int ExpiresIn { get; }
}