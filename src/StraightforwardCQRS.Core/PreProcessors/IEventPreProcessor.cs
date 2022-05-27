using StraightforwardCQRS.Core.Events;

namespace StraightforwardCQRS.Core.PreProcessors;

public interface IEventPreProcessor<in TEvent> : IRequestPreProcessor<TEvent> where TEvent : class, IEvent
{
}