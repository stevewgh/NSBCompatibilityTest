namespace TestConsole
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using System.Configuration;

    class EndpointMappingsProvideConfiguration : IProvideConfiguration<UnicastBusConfig>
    {
        static UnicastBusConfig instance = null;

        public UnicastBusConfig GetConfiguration()
        {
            if(instance != null)
            {
                return instance;
            }

            // read from existing config
            var config = (UnicastBusConfig)ConfigurationManager.GetSection(typeof(UnicastBusConfig).Name);
            if (config == null)
            {
                // create new config if it doesn't exist
                config = new UnicastBusConfig
                {
                    MessageEndpointMappings = new MessageEndpointMappingCollection()
                };
            }
            // append mapping to config
            var endpointMapping = new MessageEndpointMapping
            {
                Namespace = "Contracts.Commands",
                AssemblyName = "Contracts",
                Endpoint = "Endpoint4712"
            };
            config.MessageEndpointMappings.Add(endpointMapping);
            return instance = config;
        }
    }
}