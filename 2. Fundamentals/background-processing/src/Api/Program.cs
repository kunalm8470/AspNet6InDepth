using Api.Interfaces;
using Api.Services;
using System.Threading.Channels;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IInMemoryJobProcessorService, InMemoryJobProcessorService>();

// Register our background service
builder.Services.AddHostedService<InMemoryJobProcessorService>();
builder.Services.AddHostedService<IndependentBackgroundService>();

// Register channel
builder.Services.AddSingleton(Channel.CreateUnbounded<string>(new UnboundedChannelOptions
{
    SingleReader = true // To avoid race conditions, configure reader to be 1
}));

// Register channel reader
builder.Services.AddSingleton(svc =>
{
    return svc.GetRequiredService<Channel<string>>().Reader;
});

// Register channel writer
builder.Services.AddSingleton(svc =>
{
    return svc.GetRequiredService<Channel<string>>().Writer;
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
