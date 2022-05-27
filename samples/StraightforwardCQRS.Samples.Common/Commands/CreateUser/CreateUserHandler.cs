using StraightforwardCQRS.Core.Commands;

namespace StraightforwardCQRS.Samples.Common.Commands.CreateUser;

internal sealed class CreateUserHandler : ICommandHandler<CreateUser>
{
    private readonly IUserManager _userManager;

    public CreateUserHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task HandleAsync(CreateUser command, CancellationToken cancellationToken = default)
    {
        var (id, dto) = command;
        _userManager.Create(id, dto);
        return Task.CompletedTask;
    }
}