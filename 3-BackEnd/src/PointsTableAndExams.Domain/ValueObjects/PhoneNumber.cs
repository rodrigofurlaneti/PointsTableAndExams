using System.Text.RegularExpressions;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex Pattern = new(@"^\+?[1-9]\d{7,14}$", RegexOptions.Compiled);

    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber Create(string value)
    {
        var normalized = Regex.Replace(value ?? string.Empty, @"[\s\-\(\)]", "");

        if (string.IsNullOrWhiteSpace(normalized))
            throw new DomainException("Phone number cannot be empty.");

        if (!Pattern.IsMatch(normalized))
            throw new DomainException($"'{value}' is not a valid phone number.");

        return new PhoneNumber(normalized);
    }

    protected override IEnumerable<object> GetEqualityComponents() { yield return Value; }

    public override string ToString() => Value;
}
