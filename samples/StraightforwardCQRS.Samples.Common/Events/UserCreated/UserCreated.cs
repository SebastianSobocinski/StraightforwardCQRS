using StraightforwardCQRS.Core.Events;

namespace StraightforwardCQRS.Samples.Common.Events.UserCreated;

public record UserCreated(Guid Id) : IEvent;