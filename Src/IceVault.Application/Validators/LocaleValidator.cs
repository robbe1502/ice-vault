using System.Globalization;
using FluentValidation;
using FluentValidation.Validators;

namespace IceVault.Application.Validators;

public class LocaleValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & CultureTypes.SpecificCultures);
        return cultures.Any(el => el.Name == value);
    }

    public override string Name => "LocaleValidator";
}