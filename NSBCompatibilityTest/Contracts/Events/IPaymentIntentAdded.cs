namespace Contracts.Events
{
    using System;

    public interface IPaymentIntentAdded
    {
        Guid PaymentReference { get; set; }

        string PaymentType { get; set; }

        DateTime DateAdded { get; set; }
    }
}
