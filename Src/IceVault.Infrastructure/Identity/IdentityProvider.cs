using System.Text;
using IceVault.Application.Authentication.Login;
using IceVault.Common;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Identity;
using IceVault.Common.Identity.Models.Requests;
using IceVault.Common.Identity.Models.Results;
using IceVault.Common.Settings;
using IceVault.Infrastructure.Identity.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IceVault.Infrastructure.Identity;

internal class IdentityProvider : IIdentityProvider
{
    private readonly IHttpClientFactory _factory;
    private readonly IdentitySetting _settings;

    public IdentityProvider(IOptions<IdentitySetting> identitySetting, IHttpClientFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _settings = identitySetting.Value ?? throw new ArgumentNullException(nameof(identitySetting));
    }

    public async Task<TokenResult> GetTokenAsync(string email, string password)
    {
        var client = _factory.CreateClient();

        var discoveryDocument = await client.GetDiscoveryDocumentAsync(_settings.Authority);
        if (discoveryDocument.IsError) throw new BusinessException(FailureConstant.IdentityProvider.FailedDiscovery);

        var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = _settings.ClientId,
            ClientSecret = _settings.ClientSecret,
            Scope = _settings.Scope,
            UserName = email,
            Password = password
        });

        if (token.IsError) throw new BusinessException(FailureConstant.IdentityProvider.FailedToken);
        return new TokenResult(token.AccessToken, token.RefreshToken, token.ExpiresIn);
    }

    public async Task<TokenResult> RefreshTokenAsync(string token)
    {
        var client = _factory.CreateClient();

        var discoveryDocument = await client.GetDiscoveryDocumentAsync(_settings.Authority);
        if (discoveryDocument.IsError) throw new BusinessException(FailureConstant.IdentityProvider.FailedDiscovery);

        var result = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = _settings.ClientId,
            ClientSecret = _settings.ClientSecret,
            RefreshToken = token
        });

        if (result.IsError) throw new BusinessException(FailureConstant.IdentityProvider.FailedToken);
        return new TokenResult(result.AccessToken, result.RefreshToken, result.ExpiresIn);
    }

    public async Task<UserResult> GetProfileAsync(string token)
    {
        var client = _factory.CreateClient();

        var discoveryDocument = await client.GetDiscoveryDocumentAsync(_settings.Authority);
        if (discoveryDocument.IsError) throw new BusinessException(FailureConstant.IdentityProvider.FailedDiscovery);

        var result = await client.GetUserInfoAsync(new UserInfoRequest
        {
            Token = token,
            Address = discoveryDocument.UserInfoEndpoint
        });

        if (result.IsError) throw new BusinessException(FailureConstant.IdentityProvider.FailedProfileInfo);

        var claims = result.Claims.ToList();
        return new UserResult
        {
            Id = claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.Id)?.Value,
            Name = claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.FullName)?.Value,
            Email = claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.Email)?.Value,
            TimeZone = claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.TimeZone)?.Value,
            Currency = claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.Currency)?.Value,
            Locale = claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.Locale)?.Value
        };
    }

    public async Task<UserResult> GetUserById(string id)
    {
        var client = _factory.CreateClient();

        var url = $"{_settings.Authority}/api/v1/users/{id}";
        
        var response = await client.GetAsync(new Uri(url));
        if (!response.IsSuccessStatusCode) throw new BusinessException(FailureConstant.IdentityProvider.FailedUserRetrieval);

        var body = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<UserResult>(body);
        return result;
    }

    public async Task<Guid> RegisterUserAsync(string firstName, string lastName, string email, string password, string locale, string timeZone, string currency)
    {
        var client = _factory.CreateClient();

        var url = $"{_settings.Authority}/api/v1/users";
        var request = new RequestUserRequest()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Locale = locale,
            TimeZone = timeZone,
            Currency = currency,
            Password = password
        };

        var json = JsonConvert.SerializeObject(request);
        
        var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) throw new BusinessException(FailureConstant.IdentityProvider.FailedUserCreation);

        var body = await response.Content.ReadAsStringAsync();
        
        var result = JsonConvert.DeserializeObject<RegisterUserResponse>(body);
        if (result == null) throw new BusinessException(FailureConstant.IdentityProvider.FailedUserCreation);

        return result.Id;
    }
}