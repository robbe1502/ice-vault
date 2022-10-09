namespace IceVault.Presentation.Authentication.Models.Demands;

public class RegisterDemand
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Locale { get; set; }

    public string TimeZone { get; set; }

    public string Currency { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}