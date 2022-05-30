using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Samples.Common.PreProcessors;

public sealed class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : class, IRequest
{
    private readonly ILogger<LoggingPreProcessor<TRequest>> _logger;

    public LoggingPreProcessor(ILogger<LoggingPreProcessor<TRequest>> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"Started request: {request.GetType().Name}, at: {now:HH:mm:ss.fff}");
        return Task.CompletedTask;
    }
}