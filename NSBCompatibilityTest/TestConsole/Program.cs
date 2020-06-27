namespace TestConsole
{
    using Contracts;
    using Contracts.Commands;
    using NServiceBus;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Configure.Serialization.Json();

            var bus =
                Configure.With()
                .DefaultBuilder()
                .DefiningCommandsAs(Conventions.IsCommand)
                .DefiningEventsAs(Conventions.IsEvent)
                .InMemorySubscriptionStorage()
                .InMemorySagaPersister()
                .UseInMemoryTimeoutPersister()
                .UseTransport<Msmq>()
                .UnicastBus()
                .SendOnly();

            var intent = bus.CreateInstance<IAddPaymentIntent>();
            intent.PaymentReference = Guid.NewGuid();
            intent.PaymentType = "CrazyCash";

            bus.Send(intent);
        }
    }
}