using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Samples.Common.Decorators;

public sealed class CachingQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> 
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _decorated;
    private readonly ILogger<CachingQueryDecorator<TQuery, TResult>> _logger;

    public CachingQueryDecorator(IQueryHandler<TQuery, TResult> decorated, 
        ILogger<CachingQueryDecorator<TQuery, TResult>> logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to get result from cache...");
        // Logic with getting result from cache
        var existsInCache = new Random().Next(2) == 1;
        if (existsInCache)
        {
            // Eg. can use Redis here
            return default;
        }
        
        var result = await _decorated.HandleAsync(query, cancellationToken);
        return result;
    }
}