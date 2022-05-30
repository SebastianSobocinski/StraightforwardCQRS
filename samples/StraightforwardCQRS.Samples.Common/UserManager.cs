using StraightforwardCQRS.Samples.Common.Dtos;

namespace StraightforwardCQRS.Samples.Common;

public class UserManager : IUserManager
{
    private readonly List<UserDto> _users = new();
    
    public void Create(Guid id, NewUserDto user)
    {
        if (_users.Any(x => x.Id == id))
        {
            throw new Exception($"User with id {id} already exists");
        }
        
        var newUser = new UserDto(id, user.Name);
        _users.Add(newUser);
    }

    public void Update(Guid id, NewUserDto user)
    {
        var existingUser = _users.First(x => x.Id == id);
        existingUser.Update(user.Name);
    }

    public void Acknowledge(Guid id)
    {
        var user = _users.First(x => x.Id == id);
        user.Acknowledge();
    }

    public void Delete(Guid id)
    {
        var user = _users.First(x => x.Id == id);
        _users.Remove(user);
    }

    public UserDto Get(Guid id)
        => _users.FirstOrDefault(x => x.Id == id);

    public IEnumerable<UserDto> GetAll()
        => _users;
}