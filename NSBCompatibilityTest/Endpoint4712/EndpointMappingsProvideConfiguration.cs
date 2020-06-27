namespace Endpoint4712
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using System.Configuration;
    using Contracts.Commands;

    class EndpointMappingsProvideConfiguration : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            // read from existing config
            var config = (UnicastBusConfig)ConfigurationManager.GetSection(nameof(UnicastBusConfig));
            if (config == null)
            {
                // create new config if it doesn't exist
                config = new UnicastBusConfig
                {
                    MessageEndpointMappings = new MessageEndpointMappingCollection()
                };
            }
            
            // subscribe to events
            var endpointMapping = new MessageEndpointMapping
            {
                Namespace = "Contracts.Events",
                AssemblyName = "Contracts",
                Endpoint = "Endpoint4712"
            };
            config.MessageEndpointMappings.Add(endpointMapping);

            //  configure route for command
            endpointMapping = new MessageEndpointMapping
            {
                TypeFullName = typeof(ICheckIfPaymentShouldBeExpired).FullName,
                AssemblyName = "Contracts",
                Endpoint = "Endpoint730"
            };
            config.MessageEndpointMappings.Add(endpointMapping);

            return config;
        }
    }
}