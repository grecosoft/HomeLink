namespace HomeLink.Management.Domain.Entities;

public class DigitalRoom(string description)
{
    public string? Id { get; set; }
    public string Description { get; } = description;
}