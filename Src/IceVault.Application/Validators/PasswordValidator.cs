using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace IceVault.Application.Validators;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (value == null) return false;
        
        var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$");
        return regex.IsMatch(value);
    }

    public override string Name => "PasswordValidator";
}