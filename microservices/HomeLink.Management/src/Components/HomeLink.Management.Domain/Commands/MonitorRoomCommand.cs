using HomeLink.Management.Domain.Entities;
using NetFusion.Messaging.Types;

namespace HomeLink.Management.Domain.Commands;

public class MonitorRoomCommand(string description) : Command<EntityResult>
{
    public string Description { get; } = description;
    
    public string FloorId { get; set; } = null!;
}