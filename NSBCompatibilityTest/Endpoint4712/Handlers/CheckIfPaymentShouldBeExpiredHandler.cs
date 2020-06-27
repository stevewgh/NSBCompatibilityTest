namespace Endpoint4712.Handlers
{
    using System;
    using Contracts.Commands;
    using NServiceBus;

    class CheckIfPaymentShouldBeExpiredHandler : IHandleMessages<ICheckIfPaymentShouldBeExpired>
    {
        public void Handle(ICheckIfPaymentShouldBeExpired message)
        {
            Console.WriteLine($"OriginatingEndpoint: {Headers.GetMessageHeader(message, Headers.OriginatingEndpoint)} Checking if payment '{message.PaymentReference}' should be expired CorrelationID '{Headers.GetMessageHeader(message, Headers.CorrelationId)}'");
        }
    }
}