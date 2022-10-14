namespace IceVault.Common.ExceptionHandling;

public static class FailureConstant
{
    public static Failure SomethingWentWrong => new("Failures.SomethingWentWrong", "Something when wrong handling your request.");

    public static class IdentityProvider
    {
        public static Failure FailedDiscovery => new("Failures.IdentityProvider.Discovery", "Failed to retrieve the discovery document from Identity Server");

        public static Failure FailedToken => new("Failures.IdentityProvider.Token", "Failed to retrieve a valid access token from Identity Server");

        public static Failure FailedProfileInfo => new("Failures.IdentityProvider.ProfileInfo", "Failed to retrieve profile information from Identity Server");

        public static Failure FailedUserCreation => new("Failures.IdentityProvider.UserCreation", "Failed to register a new user");
    }
}