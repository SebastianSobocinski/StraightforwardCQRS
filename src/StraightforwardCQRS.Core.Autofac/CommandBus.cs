using Autofac;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core.Autofac;

internal sealed class CommandBus : ICommandBus
{
    private readonly IComponentContext _context;

    public CommandBus(IComponentContext context)
    {
        _context = context;
    }

    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
    {
        if (command is null)
        {
            return;
        }
        
        if (!_context.TryResolve(out ICommandHandler<TCommand> handler))
        {
            throw new InvalidOperationException($"Couldn't find handler for {command.GetType().Name}");
        }

        var preProcessors = _context.Resolve<IEnumerable<IRequestPreProcessor<TCommand>>>();
        var postProcessors = _context.Resolve<IEnumerable<IRequestPostProcessor<TCommand>>>();
        var requestPipeline = new CommandPipeline<TCommand>(preProcessors, postProcessors, handler);
        await requestPipeline.ProcessAsync(command, cancellationToken);
    }
}