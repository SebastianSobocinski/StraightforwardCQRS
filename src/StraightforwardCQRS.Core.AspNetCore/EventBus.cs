using Microsoft.Extensions.DependencyInjection;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core.AspNetCore;

internal sealed class EventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public EventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default, bool canBeRunInParallel = true) 
        where TEvent : class, IEvent
    {
        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
        var preProcessors = scope.ServiceProvider.GetServices<IRequestPreProcessor<TEvent>>();
        var postProcessors = scope.ServiceProvider.GetServices<IRequestPostProcessor<TEvent>>();
        var requestPipeline = new EventPipeline<TEvent>(preProcessors, postProcessors, handlers);
        await requestPipeline.ProcessAsync(@event, cancellationToken, canBeRunInParallel);
    }
}