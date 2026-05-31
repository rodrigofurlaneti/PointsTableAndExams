namespace PointsTableAndExams.Domain.Common;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; private init; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Entity() => Id = Guid.NewGuid();
    protected Entity(Guid id) => Id = id;

    protected void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    protected void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;

    public bool Equals(Entity? other) => other is not null && Id == other.Id;
    public override bool Equals(object? obj) => Equals(obj as Entity);
    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);
    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
}
