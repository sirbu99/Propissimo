using Propissimo.Domain.Common;
using Propissimo.Domain.Enums;
using Propissimo.Domain.ValueObjects;

namespace Propissimo.Domain.UnitTests.ValueObjects;

public class IbanTests
{
    [Theory]
    [InlineData("RO49AAAA1B31007593840000")]
    [InlineData("DE89370400440532013000")]
    [InlineData("GB82WEST12345698765432")]
    public void Create_WithValidIban_ReturnsIban(string ibanString)
    {
        // Act
        var iban = Iban.Create(ibanString);

        // Assert
        Assert.Equal(ibanString, iban.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyOrNullIban_ThrowsDomainException(string? ibanString)
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Iban.Create(ibanString!));
        Assert.Equal(DomainExceptionCode.IbanRequired, ex.Code);
    }

    [Theory]
    [InlineData("RO49AAAA")]
    [InlineData("RO49AAAA1B31007593840000RO49AAAA1B31007593")]
    [InlineData("4949AAAA1B31007593840000")]
    [InlineData("ROAAAAAA1B31007593840000")]
    [InlineData("RO49AAAA-B31007593840000")]
    public void Create_WithInvalidFormat_ThrowsDomainException(string ibanString)
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Iban.Create(ibanString));
        Assert.Equal(DomainExceptionCode.IbanInvalidFormat, ex.Code);
    }

    [Fact]
    public void Create_WithWrongCheckDigits_ThrowsDomainException()
    {
        // Arrange
        var ibanString = "RO48AAAA1B31007593840000";

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Iban.Create(ibanString));
        Assert.Equal(DomainExceptionCode.IbanInvalidChecksum, ex.Code);
    }

    [Theory]
    [InlineData("RO49 AAAA 1B31 0075 9384 0000")]
    [InlineData("ro49aaaa1b31007593840000")]
    [InlineData("  ro49 AAAA 1b31\t0075 9384 0000 ")]
    public void Create_WithSpacesOrLowerCase_NormalizesIban(string ibanString)
    {
        // Act
        var iban = Iban.Create(ibanString);

        // Assert
        Assert.Equal("RO49AAAA1B31007593840000", iban.Value);
    }

    [Fact]
    public void Equals_SameIbanWrittenDifferently_AreEqual()
    {
        // Arrange
        var first = Iban.Create("RO49 AAAA 1B31 0075 9384 0000");
        var second = Iban.Create("RO49AAAA1B31007593840000");

        // Act & Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        var iban = Iban.Create("RO49AAAA1B31007593840000");

        // Act & Assert
        Assert.Equal("RO49AAAA1B31007593840000", iban.ToString());
    }
}
