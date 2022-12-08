using System.Linq.Expressions;
using Autofac.Extras.Moq;
using IceVault.Application.Repositories;
using IceVault.Application.Repositories.Entities;
using IceVault.Common.Events;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;
using IceVault.Common.Settings;
using IceVault.Infrastructure.BackgroundJobs;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Xunit;

namespace IceVault.Infrastructure.Test.BackgroundJobs;

public class ProcessOutboxMessageJobTest : IDisposable
{
    private readonly AutoMock _mock;
    private readonly ProcessOutboxMessageJob _processor;

    public ProcessOutboxMessageJobTest()
    {
        _mock = AutoMock.GetLoose();
        _mock.Mock<IOptions<PersistenceSetting>>().Setup(el => el.Value).Returns(new PersistenceSetting());
        
        _processor = _mock.Create<ProcessOutboxMessageJob>();
    }

    [Fact]
    public async Task Execute_ShouldDispatchCorrectlyWhenUnProcessedMessageWasFound_Test()
    {
        var failures = new List<Failure>() { FailureConstant.SomethingWentWrong };
        var domainEvent = new CommandFailedEvent(Guid.NewGuid().ToString(), failures, "123", "John Doe");

        var settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        var payload = JsonConvert.SerializeObject(domainEvent, settings);
        
        var message = new OutboxMessage(domainEvent.CorrelationId, domainEvent.GetType().AssemblyQualifiedName, payload, "123", "John Doe");
        var messages = new List<OutboxMessage>() { message };

        _mock.Mock<IUnitOfWork>()
            .Setup(el => el.OutboxMessageRepository.FindAll(It.IsAny<Expression<Func<OutboxMessage,bool>>>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);
        
        var context = _mock.Mock<IJobExecutionContext>();
        await _processor.Execute(context.Object);
        
        _mock.Mock<IEventDispatcher>().Verify(el => el.Dispatch(It.IsAny<CommandFailedEvent>(), It.IsAny<string>()), Times.Once);
    }
    
    public void Dispose()
    {
        _mock?.Dispose();
    }
}