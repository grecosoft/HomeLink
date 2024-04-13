namespace HomeLink.Management.Domain.Entities;

public class DigitalFloor(int level)
{
    public string? Id { get; set; }
    public int Level { get; } = level;
}