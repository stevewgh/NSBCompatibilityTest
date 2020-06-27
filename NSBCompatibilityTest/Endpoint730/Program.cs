namespace Endpoint730
{
    using Contracts.Events;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using System.Threading.Tasks;
    using Contracts.Commands;

    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .UseNServiceBus(hostBuilderContext =>
                {
                    var config = new EndpointConfiguration("Endpoint730");
                    config.SendFailedMessagesTo("error730");
                    config.UseSerialization<NewtonsoftSerializer>().Settings(new Newtonsoft.Json.JsonSerializerSettings());

                    config.EnableInstallers();

                    var persistence = config.UsePersistence<NHibernatePersistence>();
                    // persistence.DisableSchemaUpdate();

                    config.Conventions()
                        .DefiningCommandsAs(Contracts.Conventions.IsCommand)
                        .DefiningEventsAs(Contracts.Conventions.IsEvent);

                    config.AutoSubscribe();

                    var transport = config.UseTransport<MsmqTransport>();
                    transport.Routing().RegisterPublisher(typeof(IPaymentIntentAdded), "Endpoint4712");
                    transport.Routing().RouteToEndpoint(typeof(ICheckIfPaymentShouldBeExpired), "Endpoint4712");

                    return config;
                }
                )
                .UseWindowsService()
                .Build();

            await host.RunAsync();
        }
    }
}
