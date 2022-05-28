using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Samples.Common.Events.UserUpdated;

namespace StraightforwardCQRS.Samples.Common.Commands.UpdateUser;

internal sealed class UpdateUserHandler : ICommandHandler<UpdateUser>
{
    private readonly IUserManager _userManager;
    private readonly IEventBus _eventBus;

    public UpdateUserHandler(IUserManager userManager, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
    }

    public UpdateUserHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task HandleAsync(UpdateUser command, CancellationToken cancellationToken = default)
    {
        var (id, dto) = command;
        _userManager.Update(id, dto);
        await _eventBus.PublishAsync(new UserUpdated(id, dto.Name), cancellationToken, canBeRunInParallel: false);
    }
}