using IceVault.Common.Messaging;

namespace IceVault.Common.Events.Authentication;

public class RegisteredEvent : IEvent
{
    public RegisteredEvent(string correlationId, string id, string fullName, string email, string locale)
    {
        Id = id;
        CorrelationId = correlationId;
        FullName = fullName;
        Email = email;
        Locale = locale;
    }


    public string Id { get; }
    
    public string FullName { get; }

    public string Email { get; }

    public string Locale { get; }

    public string CorrelationId { get; }
    
    public string UserId => Id;
}