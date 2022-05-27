using Autofac;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Autofac.DependencyInjection;

internal sealed class EventBus : IEventBus
{
    private readonly IComponentContext _context;

    public EventBus(IComponentContext context)
    {
        _context = context;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default, bool canBeRunInParallel = true) 
        where TEvent : class, IEvent
    {
        var preProcessors = _context.Resolve<IEnumerable<IRequestPreProcessor<TEvent>>>();
        var postProcessors = _context.Resolve<IEnumerable<IRequestPostProcessor<TEvent>>>();
        var handlers = _context.Resolve<IEnumerable<IEventHandler<TEvent>>>();
        var requestPipeline = new EventPipeline<TEvent>(preProcessors, postProcessors, handlers);
        await requestPipeline.ProcessAsync(@event, cancellationToken, canBeRunInParallel);
    }
}