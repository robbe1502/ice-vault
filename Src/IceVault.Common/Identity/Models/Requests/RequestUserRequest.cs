namespace IceVault.Common.Identity.Models.Requests;

public class RequestUserRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Currency { get; set; }

    public string TimeZone { get; set; }

    public string Locale { get; set; }

    public string Password { get; set; }
}