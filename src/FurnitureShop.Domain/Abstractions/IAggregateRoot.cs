namespace FurnitureShop.Domain.Abstractions;

public interface IAggregateRoot
{
    void ClearDomainEvents();

    IReadOnlyList<DomainEvent> GetDomainEvents();
}