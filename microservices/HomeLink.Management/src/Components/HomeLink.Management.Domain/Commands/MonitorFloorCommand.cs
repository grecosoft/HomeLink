using HomeLink.Management.Domain.Entities;
using NetFusion.Messaging.Types;

namespace HomeLink.Management.Domain.Commands;

// Creates or updates a floor associated with a home for monitoring.
public class MonitorFloorCommand(int level): Command<EntityResult>
{
    public int Level { get; } = level;

    public string HomeId { get; set; } = null!;
    public string? RelationshipId { get; set; } 
    
    public bool IsNewSetup => RelationshipId == null;
}