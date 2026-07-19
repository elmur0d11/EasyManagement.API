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
                // Reading all the information about logging from the appsettings.json file
                loggerConfig.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
