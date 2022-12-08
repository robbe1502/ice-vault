namespace IceVault.Common.Messaging;

public interface IEvent : IMessage
{
    string CorrelationId { get; }
    
    string UserId { get; }
    
    string UserName { get; }
}