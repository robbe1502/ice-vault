using FluentValidation;
using FluentValidation.Validators;
using NodaTime.TimeZones;

namespace IceVault.Application.Validators;

public class TimeZoneValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        var timeZones = TzdbDateTimeZoneSource.Default.ZoneLocations?.ToList() ?? new List<TzdbZoneLocation>();
        return timeZones.Any(el => el.ZoneId == value);
    }

    public override string Name => "TimeZoneValidator";
}