using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.NLogIntegration.Logging;
using NLog.Config;
using NLog.Targets;
using Topshelf;
using Topshelf.Logging;

namespace Responser
{
    class Program
    {
        static int Main(string[] args)
        {
            ConfigureNLog();
            NLogLogger.Use();
            NLogLogWriterFactory.Use();

            return (int) HostFactory.Run(x => x.Service<ResponderService>());
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
}
