namespace HomeLink.Management.Domain.Entities;

public class Address(string street, string city, string state, string zip)
{
    public string Street { get; } = street;
    public string City { get; } = city;
    public string State { get; } = state;
    public string Zip { get; } = zip;
}