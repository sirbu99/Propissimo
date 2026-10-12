using Propissimo.Domain.Common;
using Propissimo.Domain.Enums;
using Propissimo.Domain.ValueObjects;

namespace Propissimo.Domain.UnitTests.ValueObjects;

public class MoneyTests
{
    [Theory]
    [InlineData(Currency.EUR)]
    [InlineData(Currency.RON)]
    public void Create_WithValidAmountAndCurrency_ReturnsMoney(Currency currency)
    {
        // Arrange
        var amount = 100m;

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_ThrowsDomainException()
    {
        // Arrange
        var amount = -100m;
        var currency = Currency.EUR;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Money.Create(amount, currency));
        Assert.Equal(DomainExceptionCode.MoneyNegativeAmount, ex.Code);
    }

    [Fact]
    public void Create_WithInvalidCurrency_ThrowsDomainException()
    {
        // Arrange
        var amount = 100m;
        var currency = (Currency)999;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Money.Create(amount, currency));
        Assert.Equal(DomainExceptionCode.MoneyInvalidCurrency, ex.Code);
    }

    [Fact]
    public void Create_WithTooManyDecimals_ThrowsDomainException()
    {
        // Arrange
        var amount = 100.123m;
        var currency = Currency.EUR;

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Money.Create(amount, currency));
        Assert.Equal(DomainExceptionCode.MoneyTooManyDecimals, ex.Code);
    }

    [Fact]
    public void Create_WithTwoDecimals_DoesNotThrow()
    {
        // Arrange
        var amount = 100.12m;
        var currency = Currency.EUR;

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Theory]
    [InlineData(Currency.EUR)]
    [InlineData(Currency.RON)]
    public void Create_WithZeroAmount_DoesNotThrow(Currency currency)
    {
        // Arrange
        var amount = 0m;

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Equals_SameAmountAndCurrency_AreEqual()
    {
        // Arrange
        var first = Money.Create(100m, Currency.RON);
        var second = Money.Create(100m, Currency.RON);

        // Act & Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void Equals_DifferentCurrency_AreNotEqual()
    {
        // Arrange
        var ron = Money.Create(100m, Currency.RON);
        var eur = Money.Create(100m, Currency.EUR);

        // Act & Assert
        Assert.NotEqual(ron, eur);
    }
}