namespace HomeLink.Management.Domain.Entities;

public class DigitalHome(string owner, string yearBuilt, Address address)
{
    public string? Id { get; set; }
    public string Owner { get; } = owner;
    public string YearBuilt { get; } = yearBuilt;
    public Address Address { get; set; } = address;
}