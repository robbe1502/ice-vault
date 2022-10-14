namespace IceVault.Application.Authentication.Profile;

public class ProfileInformationResult
{
    public ProfileInformationResult(string id, string name, string email, string locale, string timezone, string currency)
    {
        Id = id;
        Name = name;
        Email = email;
        Locale = locale;
        TimeZone = timezone;
        Currency = currency;
    }

    public string Id { get; }
    
    public string Name { get; }
    
    public string Email { get; }
    
    public string Locale { get; }
    
    public string TimeZone { get; }

    public string Currency { get; }
}