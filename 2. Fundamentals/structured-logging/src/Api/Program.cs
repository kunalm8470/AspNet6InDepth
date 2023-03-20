WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Get the connection string details
string applicationInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];

// Configure Logging
builder.Services.AddLogging(b =>
{
    // Register the Azure App Insights as a logs provider (where to persist the logs)
    b.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
        {
            config.ConnectionString = applicationInsightsConnectionString;
        },
        configureApplicationInsightsLoggerOptions: (options) =>
        {
            // We want to default configuration
        }
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
