using IceVault.Application.Repositories.Entities;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Repositories.Entities;

public class OutboxMessageConsumerTest
{
    [Fact]
    public void Constructor_ShouldAssignIncomingPropertiesCorrectly_Test()
    {
        var name = Guid.NewGuid().ToString();

        var consumer = new OutboxMessageConsumer(name);
        
        consumer.Name.ShouldBe(name);
    }
}