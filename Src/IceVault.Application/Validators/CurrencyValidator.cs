using System.Globalization;
using FluentValidation;
using FluentValidation.Validators;

namespace IceVault.Application.Validators;

public class CurrencyValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var currencies = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(el => el.LCID).Distinct()
            .Select(el => new RegionInfo(el))
            .GroupBy(el => el.ISOCurrencySymbol)
            .Select(el => el.First())
            .Select(el => el.ISOCurrencySymbol);

        return currencies.Any(el => el == value);
    }

    public override string Name => "CurrencyValidator";
}