namespace Endpoint4712
{
    using Contracts;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint,
                                  IWantCustomInitialization,
                                  AsA_Publisher
    {
        public void Init()
        {
            Configure.Serialization.Json();

            var config =
                Configure.With()
                .DefaultBuilder()
                .DefineEndpointName("Endpoint4712")
                .DefiningCommandsAs(Conventions.IsCommand)
                .DefiningEventsAs(Conventions.IsEvent)
                .UseNHibernateTimeoutPersister()
                .UseNHibernateSubscriptionPersister()
                .UseTransport<Msmq>()
                .UnicastBus();
        }
    }
}