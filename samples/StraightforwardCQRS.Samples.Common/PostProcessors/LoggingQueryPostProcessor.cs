using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Samples.Common.PostProcessors;

public sealed class LoggingQueryPostProcessor<TQuery> : IQueryPostProcessor<TQuery> where TQuery: class, IQuery
{
    private readonly ILogger<LoggingQueryPostProcessor<TQuery>> _logger;

    public LoggingQueryPostProcessor(ILogger<LoggingQueryPostProcessor<TQuery>> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(TQuery request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"Finished query: {request.GetType().Name}, at: {now:HH:mm:ss.fff}");
        return Task.CompletedTask;
    }
}