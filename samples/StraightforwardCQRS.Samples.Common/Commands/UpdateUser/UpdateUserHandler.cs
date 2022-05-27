using StraightforwardCQRS.Core.Commands;

namespace StraightforwardCQRS.Samples.Common.Commands.UpdateUser;

internal sealed class UpdateUserHandler : ICommandHandler<UpdateUser>
{
    private readonly IUserManager _userManager;

    public UpdateUserHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task HandleAsync(UpdateUser command, CancellationToken cancellationToken = default)
    {
        var (id, dto) = command;
        _userManager.Update(id, dto);
        return Task.CompletedTask;
    }
}