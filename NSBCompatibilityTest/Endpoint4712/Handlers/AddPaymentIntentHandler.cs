namespace Endpoint4712.Handlers
{
    using Contracts.Commands;
    using Contracts.Events;
    using NServiceBus;
    using System;

    class AddPaymentIntentHandler : IHandleMessages<IAddPaymentIntent>
    {
        private readonly IBus bus;

        public AddPaymentIntentHandler(IBus bus)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public void Handle(IAddPaymentIntent message)
        {
            var intentAddedEvent = this.bus.CreateInstance<IPaymentIntentAdded>();

            intentAddedEvent.DateAdded = DateTime.UtcNow;
            intentAddedEvent.PaymentReference = message.PaymentReference;
            intentAddedEvent.PaymentType = message.PaymentType;

            this.bus.Publish(intentAddedEvent);
        }
    }
}
