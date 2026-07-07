using Serilog;
using System.Runtime.CompilerServices;

namespace EasyManagement.API.Services
{
    public static class AppExtension
    {
        public static void SerilogConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig.WriteTo.Console();
            });
        }
    }
}
