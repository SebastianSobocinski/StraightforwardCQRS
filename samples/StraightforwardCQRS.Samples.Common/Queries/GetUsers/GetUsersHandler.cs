using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common.Queries.GetUsers;

internal sealed class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
{
    private readonly IUserManager _userManager;

    public GetUsersHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task<IEnumerable<UserDto>> HandleAsync(GetUsers query, CancellationToken cancellationToken = default)
    {
        var users = _userManager.GetAll();
        return Task.FromResult(users);
    }
}