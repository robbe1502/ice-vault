using IceVault.Common.Identity;
using IceVault.Common.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace IceVault.Infrastructure.Messaging;

internal class CommandBus : ICommandBus
{
    private readonly IServiceScopeFactory _factory;

    public CommandBus(IServiceScopeFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task Send(ICommand command)
    {
        using var scope = _factory.CreateScope();

        var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        var accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

        var token = accessor.HttpContext.Request.Headers[HeaderNames.Authorization];

        var userId = accessor.HttpContext.User.Claims.SingleOrDefault(el => el.Type == IceVaultClaimConstant.Id)?.Value;
        var fullName = accessor.HttpContext.User.Claims.SingleOrDefault(el => el.Type == IceVaultClaimConstant.FullName)?.Value;
        var locale = accessor.HttpContext.User.Claims.SingleOrDefault(el => el.Type == IceVaultClaimConstant.Locale)?.Value;
        var timeZone = accessor.HttpContext.User.Claims.SingleOrDefault(el => el.Type == IceVaultClaimConstant.TimeZone)?.Value;
        var currency = accessor.HttpContext.User.Claims.SingleOrDefault(el => el.Type == IceVaultClaimConstant.Currency)?.Value;

        var envelope = Envelope.Create(command, token, userId, fullName, locale, timeZone, currency);
        await dispatcher.Dispatch(envelope);
    }
}
