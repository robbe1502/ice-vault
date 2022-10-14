using IceVault.Common.Identity;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Profile;

public class ProfileInformationQueryHandler : IQueryHandler<ProfileInformationQuery, ProfileInformationResult>
{
    private readonly IIdentityProvider _provider;

    public ProfileInformationQueryHandler(IIdentityProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public async Task<ProfileInformationResult> HandleAsync(ProfileInformationQuery query)
    {
        var result = await _provider.GetProfileAsync(query.AccessToken);
        return new ProfileInformationResult(result.Id, result.Name, result.Email, result.Locale, result.TimeZone, result.Currency);
    }
}