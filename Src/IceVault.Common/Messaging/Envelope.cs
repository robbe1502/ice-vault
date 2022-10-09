using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IceVault.Common.Messaging;

public class Envelope
{
    protected Envelope(string correlationId, string payload, string type, string accessToken, string userId, string fullName, string locale, string timeZone, string currency)
    {
        CorrelationId = correlationId;
        Payload = payload;
        Type = type;
        AccessToken = accessToken;
        UserId = userId;
        FullName = fullName;
        Locale = locale;
        TimeZone = timeZone;
        Currency = currency;
        CreatedAt = DateTime.UtcNow;
    }

    public string CorrelationId { get; private set; }

    public string Payload { get; }

    public string Type { get; }

    public string AccessToken { get; }

    public string UserId { get; }

    public string FullName { get; }

    public string Locale { get; }

    public string TimeZone { get; }

    public string Currency { get; }

    public bool IsAnonymous => string.IsNullOrWhiteSpace(AccessToken);

    public DateTime CreatedAt { get; }

    public static Envelope<ICommand> Create<T>(T command, string accessToken, string userId, string fullName, string locale, string timeZone, string currency) where T : ICommand
    {
        var correlationId = Guid.NewGuid().ToString();

        var payload = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var type = command.GetType().AssemblyQualifiedName;

        return new Envelope<ICommand>(correlationId, payload, type, accessToken, userId, fullName, locale, timeZone, currency);
    }

    public static Envelope<IEvent> Clone<T>(Envelope<ICommand> envelope, T message) where T : IEvent
    {
        var payload = JsonConvert.SerializeObject(message, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var type = message.GetType().AssemblyQualifiedName;

        return new Envelope<IEvent>(envelope.CorrelationId, payload, type, envelope.AccessToken, envelope.UserId, envelope.FullName, envelope.Locale, envelope.TimeZone, envelope.Currency);
    }
}

public class Envelope<T> : Envelope
{
    public Envelope(string correlationId, string payload, string type, string accessToken, string userId, string fullName, string locale, string timeZone, string currency) 
        : base(correlationId, payload, type, accessToken, userId, fullName, locale, timeZone, currency)
    {
    }
}