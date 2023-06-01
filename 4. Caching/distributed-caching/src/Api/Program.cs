using Api.Caching;
using Api.HttpClients;
using Api.Interfaces.Caching;
using Api.Interfaces.ThirdParty;
using StackExchange.Redis;
using System.Security.Authentication;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency inject IHttpClientFactory
builder.Services.AddHttpClient();

// Register user client
builder.Services.AddScoped<IUserClient, UserClient>();

// Register redis user service
builder.Services.AddSingleton<IRedisUserService, RedisUserService>();

// Register StackExchange.Redis connection to Azure Cache for Redis
builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
{
    string redisCacheConnectionString = builder.Configuration["ConnectionStrings:Redis"];

    ConfigurationOptions options = ConfigurationOptions.Parse(redisCacheConnectionString);

    // Set SSL protocol to TLS 1.2
    options.SslProtocols = SslProtocols.Tls12;

    // Set max retry count
    options.ConnectRetry = Convert.ToInt32(
        builder.Configuration["Redis:maxRetry"]
    );

    // Set connection retry policy
    options.ReconnectRetryPolicy = new LinearRetry(
        Convert.ToInt32(builder.Configuration["Redis:retryConnectMilliseconds"])
    );

    ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(options);

    return connectionMultiplexer;
});

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
