namespace StraightforwardCQRS.Core.Commands;

public interface ICommandBus
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;
}