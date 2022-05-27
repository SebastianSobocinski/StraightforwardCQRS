using StraightforwardCQRS.Core.Commands;

namespace StraightforwardCQRS.Core.PostProcessors;

public interface ICommandPostProcessor<in TCommand> : IRequestPostProcessor<TCommand> where TCommand : class, ICommand
{
}