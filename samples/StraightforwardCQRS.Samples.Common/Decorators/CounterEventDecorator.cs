using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.Events;

namespace StraightforwardCQRS.Samples.Common.Decorators;

public sealed class CounterEventDecorator<TEvent> : IEventHandler<TEvent> where TEvent : class, IEvent
{
    private readonly ILogger<CounterEventDecorator<TEvent>> _logger;
    private readonly IEventHandler<TEvent> _decorated;

    public CounterEventDecorator(ILogger<CounterEventDecorator<TEvent>> logger, IEventHandler<TEvent> decorated)
    {
        _logger = logger;
        _decorated = decorated;
    }

    public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        await _decorated.HandleAsync(@event, cancellationToken);
        Globals.ProcessedEvents++;
        _logger.LogInformation($"Already processed: [{Globals.ProcessedEvents}] events!");
    }
}