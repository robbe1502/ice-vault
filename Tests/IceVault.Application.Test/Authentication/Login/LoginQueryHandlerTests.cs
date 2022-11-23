using IceVault.Application.Authentication.Login;
using IceVault.Common.Identity;
using IceVault.Common.Identity.Models.Results;
using Moq;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Authentication.Login;

public class LoginQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenEmailAndPasswordAreGiven_Test()
    {
        var provider = new Mock<IIdentityProvider>();
        provider.Setup(el => el.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new TokenResult(string.Empty, string.Empty, 0));

        var handler = new LoginQueryHandler(provider.Object);

        var query = new LoginQuery("john.doe@hotmail.com", "123");
        var result = await handler.HandleAsync(query);

        provider.Verify(el => el.GetTokenAsync("john.doe@hotmail.com", "123"), Times.Once);
        result.AccessToken.ShouldBe(string.Empty);
        result.RefreshToken.ShouldBe(string.Empty);
        result.ExpiresIn.ShouldBe(0);
    }
}