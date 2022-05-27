using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core.Events;

public class EventPipeline<TEvent> where TEvent : class, IEvent
{
    private readonly RequestPipelineProcessor<TEvent> _pipelineProcessor;
    private readonly IEnumerable<IEventHandler<TEvent>> _handlers;

    public EventPipeline(IEnumerable<IRequestPreProcessor<TEvent>> preProcessors, 
        IEnumerable<IRequestPostProcessor<TEvent>> postProcessors,
        IEnumerable<IEventHandler<TEvent>> handlers)
    {
        _pipelineProcessor = new RequestPipelineProcessor<TEvent>(preProcessors, postProcessors);
        _handlers = handlers;
    }
    
    public async Task ProcessAsync(TEvent @event, CancellationToken cancellationToken, bool canBeRunInParallel)
    {
        await _pipelineProcessor.PreProcessAsync(@event, cancellationToken);
        if (canBeRunInParallel)
        {
            var tasks = _handlers.Select(x => x.HandleAsync(@event, cancellationToken));
            await Task.WhenAll(tasks);
        }
        else
        {
            foreach (var handler in _handlers)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }
        await _pipelineProcessor.PostProcessAsync(@event, cancellationToken);
    }
}