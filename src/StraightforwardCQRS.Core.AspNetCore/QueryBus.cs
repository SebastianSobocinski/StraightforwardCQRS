using Microsoft.Extensions.DependencyInjection;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.AspNetCore.DependencyInjection;

internal sealed class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;

    public QueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));
        if (method == null)
        {
            throw new InvalidOperationException($"Query handler for '{query.GetType().Name}' is invalid.");
        }

        var preProcessors = scope.ServiceProvider.GetServices<IRequestPreProcessor<IQuery<TResult>>>();
        var postProcessors = scope.ServiceProvider.GetServices<IRequestPostProcessor<IQuery<TResult>>>();
        var requestPipeline = new QueryPipeline<IQuery<TResult>, TResult>(preProcessors, postProcessors, 
            async () => await (Task<TResult>)method.Invoke(handler, new object[] {query, cancellationToken}));
        return await requestPipeline.ProcessAsync(query, cancellationToken);
    }
}