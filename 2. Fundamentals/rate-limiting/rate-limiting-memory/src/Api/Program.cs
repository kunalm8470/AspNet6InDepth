using AspNetCoreRateLimit;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// 1. Register our rate limiting service
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// 2. Store the rate limiting counter in the server's RAM
builder.Services.AddMemoryCache();

// 3. Configure rate limiting options
builder.Services.Configure<IpRateLimitOptions>((options) =>
{
    /* 
     * Rate limiting is global level for all endpoints
     * If you want to apply it one endpoint, you can turn it true
    */
    options.EnableEndpointRateLimiting = false;

    /*
     * Block requests are always blocked
     * they will not be carry forwarded
    */
    options.StackBlockedRequests = false;

    options.HttpStatusCode = StatusCodes.Status429TooManyRequests;

    //options.HttpStatusCode = (int)HttpStatusCode.TooManyRequests;

    // Client need to pass their public IP address in this custom request header
    options.RealIpHeader = "X-Real-IP";

    options.QuotaExceededResponse = new QuotaExceededResponse
    {
        ContentType = "application/json",
        Content = "{{ \"message\": \"Rate limit exceeded\", \"detail\": \"Quota exceeded: Maximum allowed request are: {0} per {1} seconds, Please try again in {2} seconds.\" }}",
        StatusCode = StatusCodes.Status429TooManyRequests
    };

    // For these IP addresses rate limiting won't apply
    /*
        options.IpWhitelist = new List<string>
        {
            "127.0.0.1", // IPv4 version of localhost,
            "::1" // IPv6
        };
    */

    // Bypass rate limiting on certain endpoints
    /*
        options.EndpointWhitelist = new List<string>
        {
            "/api/Status",
            "/api/Uptime"
        };
    */

    // Define the rate limiting rules
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*", // This rule will be applicable for all the clients
            Period = "10s",
            Limit = 2
        },

        new RateLimitRule
        {
            Endpoint = "*", // This rule will be applicable for all the clients
            Period = "5m",
            Limit = 100
        },

        new RateLimitRule
        {
            Endpoint = "*", // This rule will be applicable for all the clients
            Period = "1h",
            Limit = 1000
        },

        new RateLimitRule
        {
            Endpoint = "*", // This rule will be applicable for all the clients
            Period = "7d",
            Limit = 10000
        }
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

//4. Add the rate limiting middleware
app.UseIpRateLimiting();

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
