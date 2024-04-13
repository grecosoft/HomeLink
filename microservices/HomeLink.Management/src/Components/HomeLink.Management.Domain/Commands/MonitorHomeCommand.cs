using HomeLink.Management.Domain.Entities;
using NetFusion.Messaging.Types;

namespace HomeLink.Management.Domain.Commands;

// Creates digital-twin representing a house to be monitored by IoT devices.
public class MonitorHomeCommand(string owner, string yearBuilt, string city, string street, string state, string zip)
    : Command<EntityResult>
{
    public string Owner { get; } = owner;
    public string YearBuilt { get; } = yearBuilt;
    public string Street { get; } = street;
    public string City { get; } = city;
    public string State { get; } = state;
    public string Zip { get; } = zip;
    
    public string? Id { get; set; }
}