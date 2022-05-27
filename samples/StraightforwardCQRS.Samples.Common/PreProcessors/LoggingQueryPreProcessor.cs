using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.PreProcessors;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Samples.Common.PreProcessors;

public class LoggingQueryPreProcessor<TQuery> : IQueryPreProcessor<TQuery> where TQuery: class, IQuery
{
    private readonly ILogger<LoggingQueryPreProcessor<TQuery>> _logger;

    public LoggingQueryPreProcessor(ILogger<LoggingQueryPreProcessor<TQuery>> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(TQuery request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"Started query: {request.GetType().Name}, at: {now:HH:mm:ss.fff}");
        return Task.CompletedTask;
    }
}