using OpenTelemetry.Metrics;
using Serilog.Core;

namespace webapi.Monitoring;
public class MetricsSetup
{
    private static IConfigurationSection _otelConfig;
    public static void Init(WebApplicationBuilder builder, Logger logger)
    {
        _otelConfig = builder.Configuration.GetSection("Otel");
        if (!_otelConfig.Exists() || !_otelConfig.GetValue<bool>("Enabled"))
        {
            logger.Warning("OpenTelemetry Metrics are disabled");
            return;
        }

        builder.Services.AddOpenTelemetry().WithMetrics(metricsOpts => 
                metricsOpts.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter()
            );
    }

    public static void CreatePrometheusEndpoint(WebApplication app)
    {
        if (!_otelConfig.Exists() || !_otelConfig.GetValue<bool>("Enabled") || _otelConfig["Endpoint"] == null)
            return;

        app.UseOpenTelemetryPrometheusScrapingEndpoint();
    }
}
