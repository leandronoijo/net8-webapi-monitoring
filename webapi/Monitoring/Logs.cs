using Serilog;
using Serilog.Core;

namespace Utils
{
    public class LoggerSetup
    {
        public static Logger Init(WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Services.AddSingleton(logger);

            return logger;
        }
    }
}