namespace IceVault.Presentation.Authentication.Models.Demands;

public class RegisterDemand
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string Locale { get; init; }

    public string TimeZone { get; init; }

    public string Currency { get; init; }

    public string Password { get; init; }

    public string ConfirmPassword { get; init; }
}