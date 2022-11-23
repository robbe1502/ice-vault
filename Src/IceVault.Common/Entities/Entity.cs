namespace IceVault.Common.Entities;

public abstract class Entity
{
    public long Id { get; init; }

    public string ForeignSystemId { get; init; } = Guid.NewGuid().ToString();

    public string UserId { get; init; }
}