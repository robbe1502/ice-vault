using IceVault.Application.Authentication.Login;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Refresh;

public class RefreshTokenQuery : IQuery<LoginResult>
{
    public RefreshTokenQuery(string token)
    {
        Token = token;
    }

    public string Token { get; }
}