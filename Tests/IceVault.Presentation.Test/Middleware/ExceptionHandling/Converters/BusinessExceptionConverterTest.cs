using System.Net;
using Autofac.Extras.Moq;
using IceVault.Common.ExceptionHandling;
using IceVault.Presentation.Middleware.ExceptionHandling.Converters;
using Shouldly;
using Xunit;

namespace IceVault.Presentation.Test.Middleware.ExceptionHandling.Converters;

public class BusinessExceptionConverterTest
{
    private readonly BusinessExceptionConverter _converter;
    
    public BusinessExceptionConverterTest()
    {
        _converter = new BusinessExceptionConverter();
    }

    [Fact]
    public void CanConvert_ShouldReturnTrueWhenExceptionIsBusinessException_Test()
    {
        var result = _converter.CanConvert(new BusinessException(FailureConstant.SomethingWentWrong));
        result.ShouldBeTrue();
    }

    [Fact]
    public void CanConvert_ShouldReturnTrueWhenExceptionIsInheritedFromBusinessException_Test()
    {
        var result = _converter.CanConvert(new FakeBusinessException(FailureConstant.SomethingWentWrong));
        result.ShouldBeTrue();
    }

    [Fact]
    public void CanConvert_ShouldReturnFalseWhenExceptionIsDifferentFromBusinessException_Test()
    {
        var result = _converter.CanConvert(new ApplicationException());
        result.ShouldBeFalse();
    }

    [Fact]
    public void CanConvert_ShouldReturnFalseWhenExceptionIsNull_Test()
    {
        var result = _converter.CanConvert(null);
        result.ShouldBeFalse();
    }

    [Fact]
    public void Convert_ShouldReturnCorrectResult_Test()
    {
        var result = _converter.Convert(new BusinessException(FailureConstant.SomethingWentWrong));
        result.StatusCode.ShouldBe((int) HttpStatusCode.Conflict);
        result.Failures.Count().ShouldBe(1);

        var failure = result.Failures.First();
        failure.Code.ShouldBe(FailureConstant.SomethingWentWrong.Code);
        failure.Message.ShouldBe(FailureConstant.SomethingWentWrong.Message);
    }

    private class FakeBusinessException : BusinessException
    {
        public FakeBusinessException(Failure failure) : base(failure)
        {
        }

        public FakeBusinessException(List<Failure> failures) : base(failures)
        {
        }
    }
}