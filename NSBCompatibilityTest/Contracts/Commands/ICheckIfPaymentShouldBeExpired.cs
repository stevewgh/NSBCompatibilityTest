namespace Contracts.Commands
{
    using System;

    public interface ICheckIfPaymentShouldBeExpired
    {
        Guid PaymentReference { get; set; }
    }
}