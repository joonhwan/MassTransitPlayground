using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.NLogIntegration.Logging;
using NLog.Config;
using NLog.Targets;
using SharedLib;

namespace Requester
{
    class RequesterProgram
    {
        static void Main(string[] args)
        {
            ConfigureNLog();
            NLogLogger.Use();

            var bus = CreateBus();
            bus.Start();
            try
            {
                var address = new Uri("rabbitmq://localhost/request_service");
                var client = bus.CreateRequestClient<ISimpleRequest, ISimpleResponse>(address, TimeSpan.FromSeconds(10));

                for (;;)
                {
                    Console.Write("Enter customer id (quit exits): ");
                    string customerId = Console.ReadLine();
                    if (customerId == "quit")
                        break;

                    // this is run as a Task to avoid weird console application issues
                    Task.Run(() =>
                    {
                        var response = client.Request(new SimpleRequest(customerId)).Result;
                        Console.WriteLine("Customer Name: {0}", response.CustomerName);
                    }).Wait();
                }
            }
            catch (Exception e)
            {
                //throw;
                Console.WriteLine("OMG! {0}", e);
            }
            finally
            {
                bus.Stop();
            }
        }

        private static IBusControl CreateBus()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri("rabbitmq://localhost"), hostConfigurator =>
                {
                    hostConfigurator.Username("admin");
                    hostConfigurator.Password("admin");
                });
            });
            return bus;
        }

        private static void ConfigureNLog()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = "${date:format=yyyy/mm/dd HH\\:MM\\:ss} ${logger} ${message} ${exception:format=tostring}",
                Name = "Console",
            };
            config.AddTarget("Console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, consoleTarget));
            NLog.LogManager.Configuration = config;

        }
    }

    internal class SimpleRequest : ISimpleRequest
    {
        public SimpleRequest(string customerId)
        {
            TimeStamp = DateTime.Now;
            CustomerId = customerId;
        }

        public DateTime TimeStamp { get; private set; }
        public string CustomerId { get; private set; }
    }
}
