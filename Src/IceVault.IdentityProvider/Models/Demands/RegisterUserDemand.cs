namespace IceVault.IdentityProvider.Models.Demands;

public class RegisterUserDemand
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Currency { get; set; }

    public string TimeZone { get; set; }

    public string Locale { get; set; }

    public string Password { get; set; }
}