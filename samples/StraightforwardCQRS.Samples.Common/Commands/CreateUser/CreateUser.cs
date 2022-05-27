using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common.Commands.CreateUser;

public record CreateUser(Guid Id, NewUserDto Dto) : ICommand;
