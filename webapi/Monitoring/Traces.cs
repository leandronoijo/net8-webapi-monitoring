using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog.Core;

namespace webapi.Monitoring;
public class TracingSetup
{
    public static void Init(WebApplicationBuilder builder, Logger logger)
    {
        var otelConfig = builder.Configuration.GetSection("Otel");
        if (!otelConfig.Exists() || !otelConfig.GetValue<bool>("Enabled") || otelConfig["Endpoint"] == null)
        {
            logger.Warning("OpenTelemetry Tracing is disabled, or no endpoint is configured");
            return;
        }

        var serviceName = otelConfig["ServiceName"] ?? "webapi";

        builder.Services.AddOpenTelemetry().WithTracing(tracerOpts =>
            tracerOpts.AddSource(serviceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddAspNetCoreInstrumentation((opts) => opts.Filter = (req) => {
                    // don't trace incoming calls to /metrics
                    return !req.Request.Path.Value.Contains("/metrics");
                })
                .AddHttpClientInstrumentation((opts) => {
                    // don't trace outgoing calls to loki/api/v1/push
                    opts.FilterHttpRequestMessage = (req) => {
                        return !req.RequestUri?.ToString().Contains("/loki/api/v1/push") ?? false;
                    };
                })
                .AddEntityFrameworkCoreInstrumentation() // Add Entity Framework Core instrumentation
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(otelConfig["Endpoint"]);
                })
        );
    }
}