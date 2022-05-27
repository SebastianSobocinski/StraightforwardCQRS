using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common.Queries.GetUser;

internal sealed class GetUserHandler : IQueryHandler<GetUser, UserDto>
{
    private readonly IUserManager _userManager;

    public GetUserHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public Task<UserDto> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var user = _userManager.Get(query.Id);
        return Task.FromResult(user);
    }
}