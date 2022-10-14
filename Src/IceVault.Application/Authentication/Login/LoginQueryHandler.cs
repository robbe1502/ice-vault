using IceVault.Common.Identity;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, LoginResult>
{
    private readonly IIdentityProvider _identityProvider;

    public LoginQueryHandler(IIdentityProvider identityProvider)
    {
        _identityProvider = identityProvider ?? throw new ArgumentNullException(nameof(identityProvider));
    }

    public async Task<LoginResult> HandleAsync(LoginQuery query)
    {
        var result = await _identityProvider.GetTokenAsync(query.Email, query.Password);
        return new LoginResult(result.AccessToken, result.RefreshToken, result.ExpiresIn);
    }
}