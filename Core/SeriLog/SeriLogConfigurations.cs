using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SeriLog
{
    public static class SeriLogConfigurations
    {
        public static void Configure()  // Program.cs > Main()
        {
            Log.Logger = new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .Enrich.WithProperty("Application", "TestProject")
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .MinimumLevel.Override("System", LogEventLevel.Warning)
                 .WriteTo.File("Logs\\LogRecord.txt", rollingInterval: RollingInterval.Day)
                 .WriteTo.Console()
                 .WriteTo.Debug()
                 .MinimumLevel.Verbose()
                 .CreateLogger();
            Log.Information("WebApi Starting...");

            /* Program.cs > HostBuilder
                .ConfigureLogging((hostingContext, config) => { config.ClearProviders(); })
                .UseSerilog()
            */

            /* Startup.cs > Configure
                 app.UseSerilogRequestLogging();
            */
        }
    }
}
