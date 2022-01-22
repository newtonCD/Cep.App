using MassTransit;

namespace Cep.App.Core.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }

    protected BaseEntity()
    {
        Id = NewId.Next().ToGuid();
    }
}