using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Samples.Common.PreProcessors;

public sealed class LoggingEventPreProcessor<TEvent> : IEventPreProcessor<TEvent> where TEvent: class, IEvent
{
    private readonly ILogger<LoggingEventPreProcessor<TEvent>> _logger;

    public LoggingEventPreProcessor(ILogger<LoggingEventPreProcessor<TEvent>> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(TEvent request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"Started event: {request.GetType().Name}, at: {now:HH:mm:ss.fff}");
        return Task.CompletedTask;
    }
}