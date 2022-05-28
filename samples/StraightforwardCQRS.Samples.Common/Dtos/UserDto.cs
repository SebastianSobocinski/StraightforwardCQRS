namespace StraightforwardCQRS.Samples.Common.Dtos;

public class UserDto
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool Acknowledged { get; private set; }

    public UserDto(Guid id, string name)
    {
        Id = id;
        Name = name;
        Acknowledged = false;
    }

    public void Update(string name)
    {
        Name = name;
    }

    public void Acknowledge()
    {
        Acknowledged = true;
    }
}