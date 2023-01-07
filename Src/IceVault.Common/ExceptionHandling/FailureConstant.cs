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
        
        public static Failure FailedUserRetrieval => new("Failures.IdentityProvider.Retrieval", "Failed to retrieve a user with a specific id");
    }

    public static class MailService
    {
        public static Failure InvalidConfiguration => new("Failures.MailService.InvalidConfiguration", "Failed to send email due to invalid configuration");
    }

    public static class User
    {
        public static Failure FirstNameInvalid => new("Failures.User.FirstNameInvalid", "FirstName should contain at least on character and with a maximum of 50");
        
        public static Failure LastNameInvalid => new("Failures.User.LastNameInvalid", "LastName should contain at least on character and with a maximum of 100");
        
        public static Failure EmailInvalid => new("Failures.User.EmailInvalid", "Email is required and should be a valid email address");
        
        public static Failure TimeZoneInvalid => new("Failures.User.TimeZoneInvalid", "TimeZone is required and should be a an existing timezone");
        
        public static Failure CurrencyInvalid => new("Failures.User.CurrencyInvalid", "Currency is required and should be a valid currency. E.g EUR, USD, ...");
        
        public static Failure LocaleInvalid => new("Failures.User.LocaleInvalid", "Locale is required and should be a valid locale. E.g en-US, nl-BE");
        
        public static Failure PasswordInvalid => new("Failures.User.PasswordInvalid", "Password is required and should at least contain 8 characters, one number and both lower and uppercase and special characters.");
        
        public static Failure ConfirmPasswordInvalid => new("Failures.User.ConfirmPasswordInvalid", "Password is required and should at least contain 8 characters, one number and both lower and uppercase and special characters.");
        
        public static Failure PasswordNotMatch => new("Failures.User.PasswordNotMatch", "Password and ConfirmPassword are not matching.");
    }
}