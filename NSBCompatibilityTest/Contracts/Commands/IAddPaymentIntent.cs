namespace Contracts.Commands
{
    using System;

    public interface IAddPaymentIntent
    {
        Guid PaymentReference { get; set; }

        string PaymentType { get; set; }
    }
}
