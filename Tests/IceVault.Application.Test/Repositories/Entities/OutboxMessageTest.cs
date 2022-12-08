using Bogus;
using IceVault.Application.Repositories.Entities;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Repositories.Entities;

public class OutboxMessageTest
{
    [Fact]
    public void Constructor_ShouldAssignIncomingPropertiesCorrectly_Test()
    {
        var correlationId = Guid.NewGuid().ToString();
        var payload = Guid.NewGuid().ToString();
        
        var type = Guid.NewGuid().ToString();
        var user = Guid.NewGuid().ToString();
        
        var message = new OutboxMessage(correlationId, type, payload, user, user);
        
        message.CorrelationId.ShouldBe(correlationId);
        message.Payload.ShouldBe(payload);
        message.Type.ShouldBe(type);
        message.UserId.ShouldBe(user);
        message.UserName.ShouldBe(user);
        message.Consumers.Count.ShouldBe(0);
        message.Error.ShouldBeNull();
        message.CreatedAt.ShouldBeOfType<DateTime>();
        message.ProcessedAt.ShouldBeNull();
    }

    [Fact]
    public void Process_ShouldAssignProcessedAtProperty_WhenProcessFunctionIsInvoked_Test()
    {
        var message = GetOutboxMessage();
        
        message.Process();

        message.ProcessedAt.ShouldNotBeNull();
    }

    [Fact]
    public void AddConsumer_ShouldAddConsumerToConsumerList_Test()
    {
        var message = GetOutboxMessage();

        var consumer = new OutboxMessageConsumer("A fake consumer");
        message.AddConsumer(consumer);

        var result = message.Consumers.First();
        result.ShouldBe(consumer);
    }

    [Fact]
    public void AddConsumer_ShouldNotAddDuplicateConsumerList_Test()
    {
        var message = GetOutboxMessage();

        var consumer1 = new OutboxMessageConsumer("A fake consumer");
        var consumer2 = new OutboxMessageConsumer("A fake consumer");
        
        message.AddConsumer(consumer1);
        message.AddConsumer(consumer2);
        
        message.Consumers.Count.ShouldBe(1);
    }

    [Fact]
    public void SetError_ShouldAssignErrorProperty_WhenFunctionSetErrorFunctionIsInvoked_Test()
    {
        const string error = "An error occurred";
        
        var message = GetOutboxMessage();
        message.SetError(error);
        
        message.Error.ShouldBe(error);
    }

    private static OutboxMessage GetOutboxMessage()
    {
        var correlationId = Guid.NewGuid().ToString();
        var payload = Guid.NewGuid().ToString();
        
        var type = Guid.NewGuid().ToString();
        var user = Guid.NewGuid().ToString();
        
        return new OutboxMessage(correlationId, type, payload, user, user);
    }
}