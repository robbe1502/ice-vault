using System.Net;
using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;
using IceVault.Common.ExceptionHandling;
using IceVault.Presentation.Middleware.ExceptionHandling.Converters;
using Shouldly;
using Xunit;

namespace IceVault.Presentation.Test.Middleware.ExceptionHandling.Converters;

public class ValidationExceptionConverterTest
{
    private readonly ValidationExceptionConverter _converter;
    
    public ValidationExceptionConverterTest()
    {
        _converter = new ValidationExceptionConverter();
    }

    [Fact]
    public void CanConvert_ShouldReturnTrueWhenExceptionIsBusinessException_Test()
    {
        var failures = new List<ValidationFailure>()
        {
            new()
            {
                ErrorCode = FailureConstant.SomethingWentWrong.Code,
                ErrorMessage = FailureConstant.SomethingWentWrong.Message
            }
        };
        
        var result = _converter.CanConvert(new ValidationException(failures));
        result.ShouldBeTrue();
    }

    [Fact]
    public void CanConvert_ShouldReturnTrueWhenExceptionIsInheritedFromBusinessException_Test()
    {
        var failures = new List<ValidationFailure>()
        {
            new()
            {
                ErrorCode = FailureConstant.SomethingWentWrong.Code,
                ErrorMessage = FailureConstant.SomethingWentWrong.Message
            }
        };
        
        var result = _converter.CanConvert(new FakeValidationException(failures));
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
        var failures = new List<ValidationFailure>()
        {
            new()
            {
                ErrorCode = FailureConstant.SomethingWentWrong.Code,
                ErrorMessage = FailureConstant.SomethingWentWrong.Message
            }
        };
        
        var result = _converter.Convert(new ValidationException(failures));
        result.StatusCode.ShouldBe((int) HttpStatusCode.BadRequest);
        result.Failures.Count().ShouldBe(1);

        var failure = result.Failures.First();
        failure.Code.ShouldBe(FailureConstant.SomethingWentWrong.Code);
        failure.Message.ShouldBe(FailureConstant.SomethingWentWrong.Message);
    }

    private class FakeValidationException : ValidationException
    {
        public FakeValidationException(string message) : base(message)
        {
        }

        public FakeValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message, errors)
        {
        }

        public FakeValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) : base(message, errors, appendDefaultMessage)
        {
        }

        public FakeValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
        {
        }

        public FakeValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}