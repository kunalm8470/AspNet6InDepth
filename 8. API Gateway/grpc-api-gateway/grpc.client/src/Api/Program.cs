using Api.Services;
using Grpc.Core;
using Grpc.Net.Client;
using System.Net;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<GrpcChannel>(_ =>
{
    /* 
     * Use HTTPS URL for local testing, since gRPC can't communicate over insecure channel in .NET 6
     * See issue https://github.com/grpc/grpc-dotnet/pull/1802
    */
    string channelUrl = "https://localhost:7141";

    int maxSendMessageSize = 2 * 1024 * 1024;

    int maxRecieveMessageSize = 5 * 1024 * 1024;

    GrpcChannel channel = GrpcChannel.ForAddress(channelUrl, new GrpcChannelOptions 
    {
        MaxSendMessageSize = maxSendMessageSize,

        MaxReceiveMessageSize = maxRecieveMessageSize
    });

    return channel;
});

builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddHttpContextAccessor();

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
