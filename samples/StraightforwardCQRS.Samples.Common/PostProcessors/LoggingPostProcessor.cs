using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core;
using StraightforwardCQRS.Core.PostProcessors;

namespace StraightforwardCQRS.Samples.Common.PostProcessors;

public sealed class LoggingPostProcessor<TRequest> : IRequestPostProcessor<TRequest> where TRequest : class, IRequest
{
    private readonly ILogger<LoggingPostProcessor<TRequest>> _logger;

    public LoggingPostProcessor(ILogger<LoggingPostProcessor<TRequest>> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"Finished request: {request.GetType().Name}, at: {now:HH:mm:ss.fff}");
        return Task.CompletedTask;
    }
}