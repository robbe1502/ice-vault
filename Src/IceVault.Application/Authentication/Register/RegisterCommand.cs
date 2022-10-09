using System.Text.Json.Serialization;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Register;

public class RegisterCommand : ICommand
{
    public RegisterCommand(string firstName, string lastName, string email, string locale, string timeZone, string currency, string password, string confirmPassword)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Locale = locale;
        TimeZone = timeZone;
        Currency = currency;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    public string FirstName { get; }
    
    public string LastName { get; }
    
    public string Email { get; }
    
    public string Locale { get; }

    public string TimeZone { get; }

    public string Currency { get; }
    
    public string Password { get; }

    public string ConfirmPassword { get; }
}