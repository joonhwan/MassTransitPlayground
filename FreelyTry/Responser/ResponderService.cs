using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using SharedLib;
using Topshelf;
using Topshelf.Logging;

namespace Responser
{
    public class ResponderService : ServiceControl
    {
        private static readonly LogWriter _log = HostLogger.Get<ResponderService>();
        private IBusControl _bus;

        public bool Start(HostControl hostControl)
        {
            if (_bus != null)
                return true;

            _log.Info("Creating bus...");
            _bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri("rabbitmq://localhost"), hostConfigurator =>
                {
                    hostConfigurator.Username("admin");
                    hostConfigurator.Password("admin");
                });
                configurator.ReceiveEndpoint(host, "request_service", endpointConfigurator =>
                {
                    endpointConfigurator.Consumer<SimpleRequestConsumer>();
                });
            });
            _bus.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus?.Stop();
            _bus = null;
            return true;
        }
    }

    public class SimpleRequestConsumer : IConsumer<ISimpleRequest>
    {
        private static ILog _log = Logger.Get<SimpleRequestConsumer>();

        public Task Consume(ConsumeContext<ISimpleRequest> context)
        {
            _log.InfoFormat("Returning name for {0}", context.Message.CustomerId);
            return Task.Run(() =>
            {
                context.Respond(new SimpleResponse($"CustomerOf{context.Message.CustomerId}"));
            });
        }
    }

    public class SimpleResponse : ISimpleResponse
    {
        public SimpleResponse(string customerName)
        {
            CustomerName = customerName;
        }
        public string CustomerName { get; private set; }
    }
}