using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.ValueObjects;

public sealed class Points : ValueObject
{
    public static readonly Points Zero = new(0);
    public static readonly int DailyLimit = 300;

    public int Value { get; }

    private Points(int value) => Value = value;

    public static Points Create(int value)
    {
        if (value < 0)
            throw new DomainException("Points cannot be negative.");

        return new Points(value);
    }

    public Points Add(Points other) => new(Value + other.Value);
    public Points Subtract(Points other) => new(Math.Max(0, Value - other.Value));

    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }

    public override string ToString() => Value.ToString();
    public static implicit operator int(Points p) => p.Value;
}
