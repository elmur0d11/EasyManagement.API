using Serilog;
using Serilog.Formatting.Json;
using System.Runtime.CompilerServices;

namespace EasyManagement.API.Services
{
    public static class AppExtension
    {
        public static void SerilogConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((context, loggerConfig) =>
            {
                // Configure Serilog to log to console
                loggerConfig.WriteTo.Console();
                // Configure Serilog to log to a file in JSON format with daily rolling
                loggerConfig.WriteTo.File(new JsonFormatter(), "logs/applogs-.txt", rollingInterval: RollingInterval.Day);
            });
        }
    }
}
