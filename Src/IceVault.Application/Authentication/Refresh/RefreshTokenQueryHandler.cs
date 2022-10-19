using IceVault.Application.Authentication.Login;
using IceVault.Common.Identity;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Refresh;

public class RefreshTokenQueryHandler : IQueryHandler<RefreshTokenQuery, LoginResult>
{
    private readonly IIdentityProvider _provider;

    public RefreshTokenQueryHandler(IIdentityProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public async Task<LoginResult> HandleAsync(RefreshTokenQuery query)
    {
        var result = await _provider.RefreshTokenAsync(query.Token);
        return new LoginResult(result.AccessToken, result.RefreshToken, result.ExpiresIn);
    }
}