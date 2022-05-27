using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common.Queries.GetUser;

public record GetUser(Guid Id) : IQuery<UserDto>;