using StraightforwardCQRS.Core.Commands;

namespace StraightforwardCQRS.Core.PreProcessors;

public interface ICommandPreProcessor<in TCommand> : IRequestPreProcessor<TCommand> where TCommand : class, ICommand
{
}