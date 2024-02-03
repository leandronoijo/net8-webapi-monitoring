# .NET 8 Web API Monitoring with Prometheus, Jaeger, and Grafana Loki

## Project Overview

This repository demonstrates a .NET 8 Web API for managing a todo list, integrated with a comprehensive monitoring setup. Alongside CRUD operations for todo items, this API enriches each todo with a cat fact fetched from a remote API, showcasing HttpClient monitoring. The setup showcases OpenTelemetry for traces, Prometheus for metrics, Jaeger for distributed tracing with Elasticsearch as its storage backend, and Grafana Loki for logs. Designed for development and testing environments, this setup offers a quick way to get started with application monitoring and observability. **Note:** This is not a production-ready setup but provides a solid foundation for building upon.

## Prerequisites

- Docker and Docker Compose
- .NET 8 SDK
- Windows 10/11 with WSL or Docker Desktop (for Windows users)
- Linux/macOS with Docker installed (for non-Windows users)

## Getting Started

### Cloning the Repository

```bash
git clone https://github.com/leandronoijo/net8-webapi-monitoring
cd net8-webapi-monitoring
```

### Running the Web API

1. Navigate to the `webapi` folder.
2. Refer to the `appsettings.json` for initial configuration details. Specifically, review and adjust the URLs under the `Serilog` and `Otel` sections to connect your application to Loki and Jaeger.
3. Build and run the .NET application:

```bash
dotnet restore
dotnet build
dotnet run
```

The API will be accessible at `http://localhost:5000`.

### Setting up the Monitoring Services

1. **For WSL Users**: Ensure Docker is running through WSL.
   
   **For Docker Desktop Users**: Make sure Docker Desktop is operational.

2. Navigate to the `containerized-services` folder.
3. Adjust `.env` with the webapi's IP reachable to your docker containers
4. Launch the services using Docker Compose:

```
docker-compose up -d
```

## Monitoring Setup in .NET Application

The monitoring setup is configured within the `Monitoring` folder inside the `webapi` project, detailing the setup for Jaeger, Prometheus, and Loki:

- **Logs.cs**: Configures Serilog for logging, directing logs to the console, file, and Grafana Loki.
- **Metrics.cs**: Sets up OpenTelemetry metrics with Prometheus as the exporter, including ASP.NET Core, HttpClient, and runtime instrumentation.
- **Traces.cs**: Initializes OpenTelemetry tracing with configurations for ASP.NET Core, HttpClient, Entity Framework Core instrumentation, and exporting to an OTLP endpoint.

For extending monitoring capabilities to databases and other dependencies, check out available instrumentations on [NuGet](https://www.nuget.org/packages?q=opentelemetry.instrumentation&prerel=true&sortby=relevance). Examples include `OpenTelemetry.Instrumentation.SqlClient` for monitoring SQL Server and `OpenTelemetry.Instrumentation.MySqlData` for MySQL.

## Configuration and Usage

### Web API Configuration

The Web API's observability features are configured in `appsettings.json` and further refined in the `Monitoring` folder. These configurations enable the collection and export of logs, metrics, and traces.

### Monitoring Services Configuration

Monitoring services are pre-configured to work together, with datasources and dashboards for Prometheus, Jaeger, Loki, and Elasticsearch automatically provisioned in Grafana.

### Accessing Grafana

- Navigate to `http://localhost:3000`.
- Explore the "Errors" and "Performance Board" dashboards for insights into the application's health and performance.

## Additional Considerations

- This setup is designed for ease of use and quick start. It's not optimized for production deployments.
- For real-world applications, especially those using different databases, ensure the appropriate database instrumentation is added to the tracing setup for enhanced observability.

## Troubleshooting

- Verify network configurations if experiencing issues with container communication.
- Ensure `WEBAPI_IP` in `.env` matches the IP address used by your Docker network interface.

## Contributing

Contributions are welcome.

## License

This project is licensed under the [MIT License](LICENSE).
