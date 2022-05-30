using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Samples.Common.Events.UserCreated;

namespace StraightforwardCQRS.Samples.Common.Commands.CreateUser;

internal sealed class CreateUserHandler : ICommandHandler<CreateUser>
{
    private readonly IUserManager _userManager;
    private readonly IEventBus _eventBus;

    public CreateUserHandler(IUserManager userManager, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(CreateUser command, CancellationToken cancellationToken = default)
    {
        var (id, dto) = command;
        _userManager.Create(id, dto);
        await _eventBus.PublishAsync(new UserCreated(id), cancellationToken);
    }
}