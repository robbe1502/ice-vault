using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Profile;

public class ProfileInformationQuery : IQuery<ProfileInformationResult>
{
    public ProfileInformationQuery(string accessToken)
    {
        AccessToken = accessToken;
    }

    public string AccessToken { get; }
}