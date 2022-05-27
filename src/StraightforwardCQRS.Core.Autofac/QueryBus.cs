using Autofac;
using StraightforwardCQRS.Core;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Autofac.DependencyInjection;

internal sealed class QueryBus : IQueryBus
{
    private readonly IComponentContext _context;

    public QueryBus(IComponentContext context)
    {
        _context = context;
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        if (!_context.TryResolve(handlerType, out var handler))
        {
            throw new InvalidOperationException($"Couldn't find handler for {query.GetType().Name}");
        }
        
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));
        if (method is null)
        {
            throw new InvalidOperationException($"Inavlid query handler for {query.GetType().Name}");
        }
        
        var preProcessors = _context.Resolve<IEnumerable<IRequestPreProcessor<IQuery<TResult>>>>();
        var postProcessors = _context.Resolve<IEnumerable<IRequestPostProcessor<IQuery<TResult>>>>();
        var requestPipeline = new QueryPipeline<IQuery<TResult>, TResult>(preProcessors, postProcessors, 
            async () => await (Task<TResult>)method.Invoke(handler, new object[] {query, cancellationToken}));
        return await requestPipeline.ProcessAsync(@query, cancellationToken);
    }
}