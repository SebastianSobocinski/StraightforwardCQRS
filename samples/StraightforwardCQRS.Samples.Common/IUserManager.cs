using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common;

public interface IUserManager
{
    void Create(Guid id, NewUserDto user);

    void Update(Guid id, NewUserDto user);

    void Acknowledge(Guid id);

    void Delete(Guid id);

    UserDto Get(Guid id);

    IEnumerable<UserDto> GetAll();
}