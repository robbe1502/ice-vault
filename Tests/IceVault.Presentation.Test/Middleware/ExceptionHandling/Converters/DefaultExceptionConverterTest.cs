using System.Net;
using IceVault.Common.ExceptionHandling;
using IceVault.Presentation.Middleware.ExceptionHandling.Converters;
using Shouldly;
using Xunit;

namespace IceVault.Presentation.Test.Middleware.ExceptionHandling.Converters;

public class DefaultExceptionConverterTest
{
    private readonly DefaultExceptionConverter _converter;
    
    public DefaultExceptionConverterTest()
    {
        _converter = new DefaultExceptionConverter();
    }

    [Fact]
    public void CanConvert_ShouldReturnTrueForEveryException_Test()
    {
        var result = _converter.CanConvert(new ApplicationException());
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void Convert_ShouldReturnCorrectResult_Test()
    {
        var result = _converter.Convert(new BusinessException(FailureConstant.SomethingWentWrong));
        result.StatusCode.ShouldBe((int) HttpStatusCode.InternalServerError);
        result.Failures.Count().ShouldBe(1);

        var failure = result.Failures.First();
        failure.Code.ShouldBe(FailureConstant.SomethingWentWrong.Code);
        failure.Message.ShouldBe(FailureConstant.SomethingWentWrong.Message);
    }
}