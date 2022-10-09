using FakeItEasy;
using IceVault.Application.Authentication.Login;
using IceVault.Common.Identity;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Authentication.Login;

public class LoginQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenEmailAndPasswordAreGiven_Test()
    {
        //var provider = A.Fake<IIdentityProvider>();
        //A.CallTo(() => provider.GetTokenAsync(A.Dummy<string>(), A.Dummy<string>())).Returns(new TokenResult(string.Empty, string.Empty, 0));

        //var handler = new LoginQueryHandler(provider);

        //var query = new LoginQuery("john.doe@hotmail.com", "123");
        //var result = await handler.HandleAsync(query);

        //A.CallTo(() => provider.GetTokenAsync("john.doe@hotmail.com", "123")).MustHaveHappenedOnceExactly();
        //result.AccessToken.ShouldBe(string.Empty);
        //result.RefreshToken.ShouldBe(string.Empty);
        //result.ExpiresIn.ShouldBe(0);
    }
}