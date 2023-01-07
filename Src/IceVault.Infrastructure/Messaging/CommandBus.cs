using FluentValidation;
using IceVault.Common;
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
        var userId = accessor.HttpContext.User.Claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.Id)?.Value;
        var fullName = accessor.HttpContext.User.Claims.FirstOrDefault(el => el.Type == IceVaultConstant.Claim.FullName)?.Value;
        
        const string connectionId = "";
        var correlationId = GetCorrelationId(accessor.HttpContext);

        var validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
        if (scope.ServiceProvider.GetServices(validatorType) is IList<IValidator> validators)
        {
            var errors = validators.Where(el => el != null).Select(el => el.Validate(new ValidationContext<ICommand>(command))).SelectMany(el => el.Errors).ToList();
            if (errors.Any()) throw new ValidationException(errors);
        }
        
        var envelope = Envelope<T>.Create(command, authorization?["Bearer ".Length..], requestPath, connectionId, userId, fullName, correlationId);
        await dispatcher.Dispatch(envelope);
    }

    private static string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(IceVaultConstant.Tracing.CorrelationHeaderName, out var correlationId))
        {
            return correlationId;
        }

        return string.Empty;
    }
}
