using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common.Queries.GetUsers;

public record GetUsers : IQuery<IEnumerable<UserDto>>;