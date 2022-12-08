using Autofac.Extras.Moq;
using AutoMapper;
using IceVault.Application.Authentication.Login;
using IceVault.Application.Authentication.Profile;
using IceVault.Application.Authentication.Refresh;
using IceVault.Application.Authentication.Register;
using IceVault.Common.Messaging;
using IceVault.Presentation.Authentication;
using IceVault.Presentation.Authentication.Models.Demands;
using IceVault.Presentation.Authentication.Models.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace IceVault.Presentation.Test.Authentication;

public class AuthenticationControllerTest : IDisposable
{
    private readonly AutoMock _mock;
    private readonly AuthenticationController _controller;
    
    public AuthenticationControllerTest()
    {
        _mock = AutoMock.GetLoose();
        _controller = _mock.Create<AuthenticationController>();

        var provider = _mock.Mock<IServiceProvider>();
        provider.Setup(el => el.GetService(typeof(IAuthenticationService))).Returns(_mock.Mock<IAuthenticationService>().Object);
        
        
        var context = _mock.Mock<HttpContext>();
        context.Setup(el => el.RequestServices).Returns(provider.Object);
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context.Object
        };
    }
    
    [Fact]
    public async Task LoginAsync_ShouldDispatchLoginQuery_Test()
    {
        const string email = "john.doe@hotmail.com";
        const string password = "test";

        var result = new LoginResult("access_token", "refresh_token", 3600);
        _mock.Mock<IQueryDispatcher>().Setup(el => el.Dispatch(It.IsAny<LoginQuery>())).ReturnsAsync(result); 

        var response = await _controller.LoginAsync(new LoginRequest() { Email = email, Password = password });
        response.ShouldBe(result);
        
        _mock.Mock<IQueryDispatcher>().Verify(el => el.Dispatch(It.IsAny<LoginQuery>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldDispatchRegisterCommand_Test()
    {
        var demand = new RegisterDemand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@hotmail.com",
            Locale = "en_US",
            TimeZone = "Europe/Brussels",
            Currency = "EUR",
            Password = "Password",
            ConfirmPassword = "Password"
        };

        var command = new RegisterCommand("John", "Doe", "john.doe@hotmail.com", "en_US", "Europe/Brussels", "EUR", "Password", "Password");

        _mock.Mock<IMapper>().Setup(el => el.Map<RegisterCommand>(demand)).Returns(command);
        _mock.Mock<ICommandBus>().Setup(el => el.Send(It.IsAny<RegisterCommand>())).Returns(Task.CompletedTask);

        await _controller.RegisterAsync(demand);
        
        _mock.Mock<ICommandBus>().Verify(el => el.Send(It.IsAny<RegisterCommand>()), Times.Once);
    }

    [Fact]
    public async Task GetProfileInformation_ShouldDispatchProfileInformationQuery_Test()
    {
        var result = new ProfileInformationResult("123", "John Doe", "johndoe@hotmail.com", "en_US", "Europe/Brussels", "EUR");
        
        _mock.Mock<IQueryDispatcher>().Setup(el => el.Dispatch(It.IsAny<ProfileInformationQuery>())).ReturnsAsync(result);

        var response = await _controller.GetProfileInformation();
        response.ShouldBe(result);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldDispatchRefreshTokenQuery_Test()
    {
        const string token = "a dummy access token here";
        var result = new LoginResult("access_token", "refresh_token", 3600);

        _mock.Mock<IQueryDispatcher>().Setup(el => el.Dispatch(It.IsAny<RefreshTokenQuery>())).ReturnsAsync(result);

        var response = await _controller.RefreshTokenAsync(token);
        response.ShouldBe(result);
    }

    public void Dispose()
    {
        _mock?.Dispose();
    }
}