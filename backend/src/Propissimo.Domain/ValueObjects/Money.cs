using Propissimo.Domain.Common;
using Propissimo.Domain.Enums;

namespace Propissimo.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new DomainException(DomainExceptionCode.MoneyNegativeAmount, "Amount cannot be negative.");
        
        if (!Enum.IsDefined(currency))
            throw new DomainException(DomainExceptionCode.MoneyInvalidCurrency, "Currency is not supported.");
        
        if (decimal.Round(amount, 2) != amount)
            throw new DomainException(DomainExceptionCode.MoneyTooManyDecimals, "Amount cannot have more than 2 decimal places.");
        
        return new Money(amount, currency);
    }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}