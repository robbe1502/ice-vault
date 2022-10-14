﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IceVault.Common.Messaging;

public class Envelope<T> where T : IMessage
{
    protected Envelope(string correlationId, string payload, string type, string accessToken, string requestPath, string connectionId, string userId)
    {
        CorrelationId = correlationId;
        Payload = payload;
        Type = type;
        AccessToken = accessToken;
        RequestPath = requestPath;
        ConnectionId = connectionId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public string CorrelationId { get; private set; }

    public string Payload { get; }

    public string Type { get; }

    public string AccessToken { get; }
    
    public string RequestPath { get; }
    
    public string ConnectionId { get; }

    public string UserId { get; }

    public bool IsAnonymous => string.IsNullOrWhiteSpace(AccessToken);

    public DateTime CreatedAt { get; }

    public static Envelope<T> Create(T message, string accessToken, string requestPath, string connectionId, string userId)
    {
        var correlationId = Guid.NewGuid().ToString();

        var payload = JsonConvert.SerializeObject(message, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var type = message.GetType().AssemblyQualifiedName;

        return new Envelope<T>(correlationId, payload, type, accessToken, requestPath, connectionId, userId);
    }

    public T GetPayload()
    {
        return JsonConvert.DeserializeObject<T>(Payload);
    }
}