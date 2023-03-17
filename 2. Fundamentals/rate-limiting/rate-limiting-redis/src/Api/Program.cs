using Api.Configurations;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev_Environment_Policy", builder =>
    {
        builder
        .AllowAnyOrigin() // Access-Control-Allow-Origin: *
        .WithExposedHeaders(new[]
        {
            "X-Rate-Limit-Limit",
            "X-Rate-Limit-Remaining",
            "X-Rate-Limit-Reset"
        })
        .AllowAnyMethod(); // For allowing unsafe requests (any request which is not HTTP GET or HTTP POST) Access-Control-Allow-Method: *
    });
});

string redisConnectionString = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;

    options.InstanceName = "AspNetCoreRateLimiting:";
});

builder.Services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddRedisRateLimiting();

builder.Services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;

    options.StackBlockedRequests = false;

    options.HttpStatusCode = StatusCodes.Status429TooManyRequests;

    //options.IpWhitelist = new List<string>
    //{

    //};

    //options.EndpointWhitelist = new List<string>
    //{

    //};

    options.QuotaExceededResponse = new QuotaExceededResponse
    {
        ContentType = "application/problem+json",
        Content = "{{ \"message\": \"Rate limit exceeded\", \"detail\": \"Quota exceeded: Maximum allowed request are: {0} per {1} seconds, Please try again in {2} seconds.\" }}",
        StatusCode = StatusCodes.Status429TooManyRequests
    };

    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "10s",
            Limit = 2
        },
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "5m",
            Limit = 100
        },
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1h",
            Limit = 1000
        },
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "7d",
            Limit = 10000
        }
    };
});

WebApplication app = builder.Build();

app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("Dev_Environment_Policy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
