namespace Endpoint4712.Handlers
{
    using Contracts.Events;
    using NServiceBus;
    using System;

    class PaymentIntentAddedHandler : IHandleMessages<IPaymentIntentAdded>
    {
        private readonly IBus bus;

        public PaymentIntentAddedHandler(IBus bus)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public void Handle(IPaymentIntentAdded message)
        {
            Console.WriteLine($"A Payment intent '{message.PaymentReference}' was added! {message.DateAdded}");
        }
    }
}
