using Microsoft.Extensions.DependencyInjection;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core.AspNetCore;

internal sealed class CommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
    {
        if (command is null)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        var preProcessors = scope.ServiceProvider.GetServices<IRequestPreProcessor<TCommand>>();
        var postProcessors = scope.ServiceProvider.GetServices<IRequestPostProcessor<TCommand>>();
        var requestPipeline = new CommandPipeline<TCommand>(preProcessors, postProcessors, handler);
        await requestPipeline.ProcessAsync(@command, cancellationToken);
    }
}