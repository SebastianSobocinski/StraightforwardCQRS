using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.Commands;

namespace StraightforwardCQRS.Samples.Common.Decorators;

public sealed class UnitOfWorkCommandDecorator<TCommand> : ICommandHandler<TCommand> 
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated;
    private readonly ILogger<UnitOfWorkCommandDecorator<TCommand>> _logger;

    public UnitOfWorkCommandDecorator(ICommandHandler<TCommand> decorated, 
        ILogger<UnitOfWorkCommandDecorator<TCommand>> logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        await _decorated.HandleAsync(command, cancellationToken);
        _logger.LogInformation("Commiting transaction...");
        // Logic with commiting changes
    }
}