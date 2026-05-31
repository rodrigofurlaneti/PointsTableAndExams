using System.Text.RegularExpressions;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly Regex Pattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if (!Pattern.IsMatch(value))
            throw new DomainException($"'{value}' is not a valid email address.");

        return new Email(value.ToLowerInvariant().Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }

    public override string ToString() => Value;
}
