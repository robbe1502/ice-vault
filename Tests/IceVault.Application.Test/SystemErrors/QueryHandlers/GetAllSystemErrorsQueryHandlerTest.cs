using Autofac.Extras.Moq;
using IceVault.Application.SystemErrors.Entities;
using IceVault.Application.SystemErrors.Queries;
using IceVault.Application.SystemErrors.QueryHandlers;
using IceVault.Application.SystemErrors.Repositories;
using Moq;
using NodaTime;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.SystemErrors.QueryHandlers;

public class GetAllSystemErrorsQueryHandlerTest : IDisposable
{
    private readonly AutoMock _mock;
    private readonly GetAllSystemErrorsQueryHandler _handler;
    
    public GetAllSystemErrorsQueryHandlerTest()
    {
        _mock = AutoMock.GetLoose();
        _handler = _mock.Create<GetAllSystemErrorsQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnListOfSystemErrors_Test()
    {
        var error = new SystemError(Guid.NewGuid().ToString(), string.Empty, string.Empty, string.Empty, SystemClock.Instance.GetCurrentInstant());
        var errors = new List<SystemError> { error };

        _mock.Mock<ISystemErrorRepository>().Setup(el => el.GetSystemErrors()).ReturnsAsync(errors);

        var result = await _handler.HandleAsync(new GetAllSystemErrorsQuery());
        result.ShouldBe(errors);
    }

    public void Dispose()
    {
        _mock?.Dispose();
    }
}