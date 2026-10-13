using Propissimo.Domain.Enums;

namespace Propissimo.Domain.Common;

public sealed class DomainException : Exception
{
    public DomainExceptionCode Code { get; }

    public DomainException(DomainExceptionCode code, string message) : base(message)
    {
        Code = code;
    }
}