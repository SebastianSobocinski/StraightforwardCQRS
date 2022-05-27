using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common.Commands.UpdateUser;

public record UpdateUser(Guid Id, NewUserDto Dto) : ICommand;