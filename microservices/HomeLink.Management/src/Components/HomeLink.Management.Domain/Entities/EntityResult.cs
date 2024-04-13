using System;

namespace HomeLink.Management.Domain.Entities;

public class EntityResult
{
    public string? EntityId { get; private set; }
    public string? Message { get; private set; }
    public string? Etag { get; private set; }
    public string? RelationshipId { get; set; }
    public bool IsValid => string.IsNullOrEmpty(Message);

    private EntityResult() { }
    
    public static EntityResult For(string entityId, string? etag, Action<EntityResult>? result = null) => new() { EntityId = entityId, Etag = etag };
    public static EntityResult WithError(string message) => new() { Message = message };
}