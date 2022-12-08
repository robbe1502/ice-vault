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

    public async Task Send<T>(T command) where T : ICommand
    {
        using var scope = _factory.CreateScope();

        var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        var accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

        string authorization = accessor.HttpContext.Request.Headers[HeaderNames.Authorization];

        var requestPath = accessor.HttpContext.Request.Path.Value;
        var userId = accessor.HttpContext.User.Claims.FirstOrDefault(el => el.Type == IceVaultClaimConstant.Id)?.Value;
        var fullName = accessor.HttpContext.User.Claims.FirstOrDefault(el => el.Type == IceVaultClaimConstant.FullName)?.Value;
        
        var connectionId = "";
        
        var envelope = Envelope<T>.Create(command, authorization?["Bearer ".Length..], requestPath, connectionId, userId, fullName);
        await dispatcher.Dispatch(envelope);
    }
}
