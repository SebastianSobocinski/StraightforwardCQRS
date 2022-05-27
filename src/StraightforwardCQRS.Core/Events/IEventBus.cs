namespace StraightforwardCQRS.Core.Events;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default, bool canBeRunInParallel = true) 
        where TEvent : class, IEvent;
}