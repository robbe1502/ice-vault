using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Infrastructure.Messaging;

internal class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _provider;

    public QueryDispatcher(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

        dynamic handler = _provider.GetService(handlerType);
        if (handler == null) throw new ArgumentNullException($"Cant resolve handler for query with type ${query.GetType()}");

        return await handler.HandleAsync((dynamic) query);
    }
}