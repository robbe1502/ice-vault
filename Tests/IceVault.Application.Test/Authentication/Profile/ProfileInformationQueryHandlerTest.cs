using Autofac.Extras.Moq;
using IceVault.Application.Authentication.Profile;
using IceVault.Common.Identity;
using IceVault.Common.Identity.Models.Results;
using Moq;
using Shouldly;
using Xunit;

namespace IceVault.Application.Test.Authentication.Profile;

public class ProfileInformationQueryHandlerTest : IDisposable
{
    private readonly AutoMock _mock;
    private readonly ProfileInformationQueryHandler _handler;
    
    public ProfileInformationQueryHandlerTest()
    {
        _mock = AutoMock.GetLoose();
        _handler = _mock.Create<ProfileInformationQueryHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnProfileInformation_Test()
    {
        var result = new UserResult() { Name = "John Doe", Email = "john.doe@hotmail.com", Currency = "EUR", Locale = "en-US", Id = "123", TimeZone = "Europe/Brussels" };
        
        _mock.Mock<IIdentityProvider>().Setup(el => el.GetProfileAsync(It.IsAny<string>())).ReturnsAsync(result);

        var query = new ProfileInformationQuery("A valid access token");
        var response = await _handler.HandleAsync(query);
        
        response.Id.ShouldBe(result.Id);
        response.Name.ShouldBe(result.Name);
        response.Email.ShouldBe(result.Email);
        response.Locale.ShouldBe(result.Locale);
        response.TimeZone.ShouldBe(result.TimeZone);
        response.Currency.ShouldBe(result.Currency);
    }

    public void Dispose()
    {
        _mock?.Dispose();
    }
}