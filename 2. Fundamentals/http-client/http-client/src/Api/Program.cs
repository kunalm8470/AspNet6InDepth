using Api.DelegatingHandlers;
using Api.HttpClient;
using Api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Named client, where the string is the key
builder.Services.AddHttpClient("JsonTypicode", (httpClient) =>
{
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

// Typed client
builder.Services.AddScoped<IJsonTypicodeClient, JsonTypicodeClient>();

builder.Services.AddHttpClient<IJsonTypicodeClient, JsonTypicodeClient>((httpClient) =>
{
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
})
.AddHttpMessageHandler<CustomHeaderDelegatingHandler>();

builder.Services.AddTransient<CustomHeaderDelegatingHandler>();

var app = builder.Build();

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
