using Autofac.Extras.Moq;
using IceVault.Application.Authentication.Refresh;
using IceVault.Common.Identity;
using IceVault.Common.Identity.Models.Results;
using Moq;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Authentication.Refresh;

public class RefreshTokenQueryHandlerTest : IDisposable
{
    private readonly AutoMock _mock;
    private readonly RefreshTokenQueryHandler _handler;
    
    public RefreshTokenQueryHandlerTest()
    {
        _mock = AutoMock.GetLoose();
        _handler = _mock.Create<RefreshTokenQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnLoginResult_Test()
    {
        var tokenResult = new TokenResult("A valid access token", "A valid refresh Token", 3600);
        _mock.Mock<IIdentityProvider>().Setup(el => el.RefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(tokenResult);

        var result = await _handler.HandleAsync(new RefreshTokenQuery("A valid token"));
        result.AccessToken.ShouldBe(tokenResult.AccessToken);
        result.RefreshToken.ShouldBe(tokenResult.RefreshToken);
        result.ExpiresIn.ShouldBe(tokenResult.ExpiresIn);
    }

    public void Dispose()
    {
        _mock?.Dispose();
    }
}