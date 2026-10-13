using Propissimo.Domain.Common;
using Propissimo.Domain.Enums;

namespace Propissimo.Domain.ValueObjects;

public sealed record Iban
{
    private const int MinLength = 15;
    private const int MaxLength = 34;

    public string Value { get; }

    public static Iban Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(DomainExceptionCode.IbanRequired, "IBAN is required.");

        var normalized = string.Concat(value.Where(c => !char.IsWhiteSpace(c))).ToUpperInvariant();

        if (!HasValidFormat(normalized))
            throw new DomainException(DomainExceptionCode.IbanInvalidFormat, "IBAN format is not valid.");

        if (!HasValidChecksum(normalized))
            throw new DomainException(DomainExceptionCode.IbanInvalidChecksum, "IBAN check digits are not valid.");

        return new Iban(normalized);
    }

    public override string ToString() => Value;

    private Iban(string value)
    {
        Value = value;
    }

    private static bool HasValidFormat(string iban)
    {
        if (iban.Length < MinLength || iban.Length > MaxLength)
            return false;

        return char.IsAsciiLetterUpper(iban[0])
            && char.IsAsciiLetterUpper(iban[1])
            && char.IsAsciiDigit(iban[2])
            && char.IsAsciiDigit(iban[3])
            && iban.All(char.IsAsciiLetterOrDigit);
    }

    // ISO 13616: move the first four characters to the end, replace letters with numbers
    // (A = 10 ... Z = 35) and check that the resulting number mod 97 equals 1.
    private static bool HasValidChecksum(string iban)
    {
        var rearranged = iban[4..] + iban[..4];
        var remainder = 0;

        foreach (var c in rearranged)
        {
            remainder = char.IsAsciiDigit(c)
                ? (remainder * 10 + (c - '0')) % 97
                : (remainder * 100 + (c - 'A' + 10)) % 97;
        }

        return remainder == 1;
    }
}
