using StraightforwardCQRS.Core.Events;

namespace StraightforwardCQRS.Core.PostProcessors;

public interface IEventPostProcessor<in TEvent> : IRequestPostProcessor<TEvent> where TEvent : class, IEvent
{
}