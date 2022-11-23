using Autofac.Extras.Moq;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Settings;
using IceVault.Infrastructure.Mails;
using IceVault.Infrastructure.Mails.Makers;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace IceVault.Infrastructure.Test.Mails;

public class MailServiceTest : IDisposable
{
    private readonly AutoMock _mock;
    
    public MailServiceTest()
    {
        _mock = AutoMock.GetLoose();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Send_ShouldThrowErrorWhenHostIsEmpty_Test(string host)
    {
        _mock.Mock<IOptions<MailSetting>>().Setup(el => el.Value).Returns(new MailSetting() { Host = host });

        var service = _mock.Create<MailService>();
        var exception = Should.Throw<BusinessException>(async () => await service.Send(new RegisterEmailMaker("john.doe@hotmail.com")));

        var failure = exception.Failures[0];
        failure.Code.ShouldBe(FailureConstant.MailService.InvalidConfiguration.Code);
        failure.Message.ShouldBe(FailureConstant.MailService.InvalidConfiguration.Message);
    }

    public void Dispose()
    {
        _mock?.Dispose();
    }
}