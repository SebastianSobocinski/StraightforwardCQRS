using StraightforwardCQRS.Core.Events;

namespace StraightforwardCQRS.Samples.Common.Events.UserCreated;

internal sealed class UserCreatedHandler : IEventHandler<UserCreated>
{
    private readonly IUserManager _userManager;

    public UserCreatedHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task HandleAsync(UserCreated @event, CancellationToken cancellationToken = default)
    {
        _userManager.Acknowledge(@event.Id);
        return Task.CompletedTask;
    }
}