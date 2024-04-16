namespace Domain.Abstractions;

public class Entity<TEntityId>
{
    private readonly List<DomainEvent> _domainEvents = new();
    
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    protected Entity()
    {
        // For EF Core
    }

    public TEntityId Id { get; private set; }
    
    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyList<DomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();
}