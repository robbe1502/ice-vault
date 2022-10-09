using IceVault.Common.Messaging;

namespace IceVault.Common.Events.Authentication;

public class RegisteredEvent : IEvent
{
    public RegisteredEvent(string fullName, string email, string locale)
    {
        FullName = fullName;
        Email = email;
        Locale = locale;
    }


    public string FullName { get; }

    public string Email { get; }

    public string Locale { get; }
}