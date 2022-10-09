using IceVault.Common.Identity;
using IceVault.Common.Settings;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace IceVault.Infrastructure.Identity;

internal class IdentityProvider : IIdentityProvider
{
    private readonly IHttpClientFactory _factory;
    private readonly IdentitySetting _identitySetting;

    public IdentityProvider(IOptions<IdentitySetting> identitySetting, IHttpClientFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _identitySetting = identitySetting.Value ?? throw new ArgumentNullException(nameof(identitySetting));
    }

    public async Task<TokenResult> GetTokenAsync(string email, string password)
    {
        var client = _factory.CreateClient();

        var discoveryDocument = await client.GetDiscoveryDocumentAsync(_identitySetting.Authority);
        if (discoveryDocument.IsError) throw new Exception($"Unable to get the Discovery Endpoint. Authority ${_identitySetting.Authority}");

        var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = _identitySetting.ClientId,
            ClientSecret = _identitySetting.ClientSecret,
            Scope = _identitySetting.Scope,
            UserName = email,
            Password = password
        });
;   
        return new TokenResult(token.AccessToken, token.RefreshToken, token.ExpiresIn);
    }
}