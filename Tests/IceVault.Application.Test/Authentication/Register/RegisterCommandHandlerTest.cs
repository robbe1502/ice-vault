using Autofac.Extras.Moq;
using IceVault.Application.Authentication.Register;
using IceVault.Common.Events.Authentication;
using IceVault.Common.Identity;
using IceVault.Common.Messaging;
using Moq;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Authentication.Register;

public class RegisterCommandHandlerTest : IDisposable
{
    private readonly AutoMock _mock;
    private readonly RegisterCommandHandler _handler;
    
    public RegisterCommandHandlerTest()
    {
        _mock = AutoMock.GetLoose();
        _handler = _mock.Create<RegisterCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldDispatchRegisteredEvent_WhenUserIsRegistered_Test()
    {
        RegisteredEvent domainEvent = null;
        var userId = Guid.NewGuid();

        _mock.Mock<IEventBus>().Setup(el => el.Publish(It.IsAny<RegisteredEvent>()))
             .Callback<IEvent>((@event) => domainEvent = (RegisteredEvent) @event)
             .Returns(Task.CompletedTask);
        
        _mock.Mock<IIdentityProvider>()
            .Setup(el => el.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(userId);

        var command = new RegisterCommand("John", "Doe", "john.doe@hotmail.com", "en-US", "Europe/Brussels", "EUR", "123", "123");
        
        var envelope = Envelope<RegisterCommand>.Create(command, "", "", "", "", "");
        await _handler.HandleAsync(envelope);
        
        _mock.Mock<IEventBus>().Verify(el => el.Publish(It.IsAny<RegisteredEvent>()), Times.Once);
        
        domainEvent.CorrelationId.ShouldBe(envelope.CorrelationId);
        domainEvent.FullName.ShouldBe($"{command.FirstName} {command.LastName}");
        domainEvent.Locale.ShouldBe(command.Locale);
        domainEvent.UserId.ShouldBe(userId.ToString());
        domainEvent.Email.ShouldBe(command.Email);
    }

    public void Dispose()
    {
        _mock?.Dispose();
    }
}