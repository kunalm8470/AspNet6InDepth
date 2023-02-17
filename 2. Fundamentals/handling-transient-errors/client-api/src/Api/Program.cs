using Api.Interfaces;
using Api.Middlewares;
using Api.Policies;
using Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Register the policy objects
builder.Services.AddSingleton<IClientRetryPolicy, ClientRetryPolicy>();

// Register the named HTTP client and its service
builder.Services.AddTransient<IRandomValueService, RandomValueService>();

builder.Services.AddHttpClient<IRandomValueService, RandomValueService>((client) =>
{
    string baseAddress = builder.Configuration.GetValue<string>("ExternalApi:BaseUrl");

    client.BaseAddress = new Uri(baseAddress);
});

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

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.Run();
