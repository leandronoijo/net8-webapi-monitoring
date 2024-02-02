using Utils;
using webapi.Monitoring;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Setup logging, tracing and metrics
var logger = LoggerSetup.Init(builder);
TracingSetup.Init(builder, logger);
MetricsSetup.Init(builder, logger);

// Add services to the container.
builder.Services.AddScoped((_) => TodoDatabaseService.GetConnection());
builder.Services.AddDbContext<TodoDBContext>();
builder.Services.AddHttpClient<CatFactService>();

//add swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.EnableAnnotations();
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
// Setup Prometheus endpoint
MetricsSetup.CreatePrometheusEndpoint(app);

// Generate Todo endpoints
TodosEndpoints.GenerateEndpoints(app);

// add swagger
app.MapSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();