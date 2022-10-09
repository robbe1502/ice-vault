using AutoMapper;
using IceVault.Application.Authentication.Register;
using IceVault.Presentation.Authentication.Models.Demands;

namespace IceVault.Presentation.Authentication.Mappers.Demands;

public class RegisterDemandMapperProfile : Profile
{
    public RegisterDemandMapperProfile()
    {
        CreateMap<RegisterDemand, RegisterCommand>()
            .ConvertUsing((demand) => new RegisterCommand(demand.FirstName, demand.LastName, demand.Email, demand.Locale, demand.TimeZone, demand.Currency, demand.Password, demand.ConfirmPassword));
    }
}