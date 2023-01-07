using FluentValidation;
using IceVault.Application.Validators;
using IceVault.Common.ExceptionHandling;

namespace IceVault.Application.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(el => el.FirstName)
            .NotEmpty()
            .WithMessage(FailureConstant.User.FirstNameInvalid.Message)
            .WithErrorCode(FailureConstant.User.FirstNameInvalid.Code);
        
        RuleFor(el => el.FirstName)
            .MaximumLength(50)
            .WithMessage(FailureConstant.User.FirstNameInvalid.Message)
            .WithErrorCode(FailureConstant.User.FirstNameInvalid.Code);
        
        RuleFor(el => el.LastName)
            .NotEmpty()
            .WithMessage(FailureConstant.User.LastNameInvalid.Message)
            .WithErrorCode(FailureConstant.User.LastNameInvalid.Code);
        
        RuleFor(el => el.LastName)
            .MaximumLength(100)
            .WithMessage(FailureConstant.User.LastNameInvalid.Message)
            .WithErrorCode(FailureConstant.User.LastNameInvalid.Code);
        
        RuleFor(el => el.Email)
            .NotEmpty()
            .WithMessage(FailureConstant.User.EmailInvalid.Message)
            .WithErrorCode(FailureConstant.User.EmailInvalid.Code);
        
        RuleFor(el => el.Email)
            .EmailAddress()
            .WithMessage(FailureConstant.User.EmailInvalid.Message)
            .WithErrorCode(FailureConstant.User.EmailInvalid.Code);

        RuleFor(el => el.TimeZone)
            .NotEmpty()
            .WithMessage(FailureConstant.User.TimeZoneInvalid.Message)
            .WithErrorCode(FailureConstant.User.TimeZoneInvalid.Code);
        
        RuleFor(el => el.TimeZone)
            .SetValidator(new TimeZoneValidator<RegisterCommand>())
            .WithMessage(FailureConstant.User.TimeZoneInvalid.Message)
            .WithErrorCode(FailureConstant.User.TimeZoneInvalid.Code);

        RuleFor(el => el.Currency)
            .NotEmpty()
            .WithMessage(FailureConstant.User.CurrencyInvalid.Message)
            .WithErrorCode(FailureConstant.User.CurrencyInvalid.Code);
        
        RuleFor(el => el.Currency)
            .MaximumLength(3)
            .WithMessage(FailureConstant.User.CurrencyInvalid.Message)
            .WithErrorCode(FailureConstant.User.CurrencyInvalid.Code);
        
        RuleFor(el => el.Currency)
            .SetValidator(new CurrencyValidator<RegisterCommand>())
            .WithMessage(FailureConstant.User.CurrencyInvalid.Message)
            .WithErrorCode(FailureConstant.User.CurrencyInvalid.Code);
        
        RuleFor(el => el.Locale)
            .NotEmpty()
            .WithMessage(FailureConstant.User.LocaleInvalid.Message)
            .WithErrorCode(FailureConstant.User.LocaleInvalid.Code);
        
        RuleFor(el => el.Locale)
            .MaximumLength(5)
            .WithMessage(FailureConstant.User.LocaleInvalid.Message)
            .WithErrorCode(FailureConstant.User.LocaleInvalid.Code);
        
        RuleFor(el => el.Locale)
            .SetValidator(new LocaleValidator<RegisterCommand>())
            .WithMessage(FailureConstant.User.LocaleInvalid.Message)
            .WithErrorCode(FailureConstant.User.LocaleInvalid.Code);
        
        RuleFor(el => el.Password)
            .NotEmpty()
            .WithMessage(FailureConstant.User.PasswordInvalid.Message)
            .WithErrorCode(FailureConstant.User.PasswordInvalid.Code);
        
        RuleFor(el => el.Password)
            .SetValidator(new PasswordValidator<RegisterCommand>())
            .WithMessage(FailureConstant.User.PasswordInvalid.Message)
            .WithErrorCode(FailureConstant.User.PasswordInvalid.Code);
        
        RuleFor(el => el.ConfirmPassword)
            .NotEmpty()
            .WithMessage(FailureConstant.User.ConfirmPasswordInvalid.Message)
            .WithErrorCode(FailureConstant.User.ConfirmPasswordInvalid.Code);
        
        RuleFor(el => el.ConfirmPassword)
            .SetValidator(new PasswordValidator<RegisterCommand>())
            .WithMessage(FailureConstant.User.ConfirmPasswordInvalid.Message)
            .WithErrorCode(FailureConstant.User.ConfirmPasswordInvalid.Code);

        RuleFor(el => el.Password)
            .Equal(el => el.ConfirmPassword)
            .WithMessage(FailureConstant.User.PasswordNotMatch.Message)
            .WithErrorCode(FailureConstant.User.PasswordNotMatch.Code);
    }
}