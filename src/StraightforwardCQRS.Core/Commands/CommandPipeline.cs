using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core.Commands;

public class CommandPipeline<TCommand> where TCommand : class, ICommand
{
    private readonly RequestPipelineProcessor<TCommand> _pipelineProcessor;
    private readonly ICommandHandler<TCommand> _handler;

    public CommandPipeline(IEnumerable<IRequestPreProcessor<TCommand>> preProcessors, 
        IEnumerable<IRequestPostProcessor<TCommand>> postProcessors, 
        ICommandHandler<TCommand> handler)
    {
        _pipelineProcessor = new RequestPipelineProcessor<TCommand>(preProcessors, postProcessors);
        _handler = handler;
    }

    public async Task ProcessAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        await _pipelineProcessor.PreProcessAsync(command, cancellationToken);
        await _handler.HandleAsync(command, cancellationToken);
        await _pipelineProcessor.PostProcessAsync(command, cancellationToken);
    }
}