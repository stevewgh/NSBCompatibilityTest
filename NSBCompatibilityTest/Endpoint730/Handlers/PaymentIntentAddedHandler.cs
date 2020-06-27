namespace Endpoint730.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Commands;
    using Contracts.Events;
    using NServiceBus;

    class PaymentIntentAddedHandler : IHandleMessages<IPaymentIntentAdded>
    {
        public async Task Handle(IPaymentIntentAdded message, IMessageHandlerContext context)
        {
            Console.WriteLine($"A Payment intent '{message.PaymentReference}' was added! {message.DateAdded}");
            Console.WriteLine("Deferring an expiry check command.");

            var options = new SendOptions();
            options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
            await context.Send<ICheckIfPaymentShouldBeExpired>(expiryCheckCommand => expiryCheckCommand.PaymentReference = message.PaymentReference, options);
        }
    }
}
