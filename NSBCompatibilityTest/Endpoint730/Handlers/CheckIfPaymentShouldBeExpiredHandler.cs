namespace Endpoint730.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Commands;
    using NServiceBus;

    class CheckIfPaymentShouldBeExpiredHandler : IHandleMessages<ICheckIfPaymentShouldBeExpired>
    {
        public Task Handle(ICheckIfPaymentShouldBeExpired message, IMessageHandlerContext context)
        {
            Console.WriteLine($"OriginatingEndpoint: {context.MessageHeaders[Headers.OriginatingEndpoint]} Checking if payment '{message.PaymentReference}' should be expired CorrelationID '{context.MessageHeaders[Headers.CorrelationId]}'");
            return Task.CompletedTask;
        }
    }
}