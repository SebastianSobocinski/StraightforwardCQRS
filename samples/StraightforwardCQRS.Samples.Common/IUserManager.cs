using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common;

public interface IUserManager
{
    public void Create(Guid id, NewUserDto user);

    public void Update(Guid id, NewUserDto user);

    public void Delete(Guid id);

    public UserDto Get(Guid id);

    public IEnumerable<UserDto> GetAll();
}