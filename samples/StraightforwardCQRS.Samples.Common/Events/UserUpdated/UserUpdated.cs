using StraightforwardCQRS.Core.Events;

namespace StraightforwardCQRS.Samples.Common.Events.UserUpdated;

public record UserUpdated(Guid Id, string Name) : IEvent;