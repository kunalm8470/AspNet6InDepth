using Azure.Storage;
using Azure.Storage.Blobs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton((serviceProvider) =>
{
    string accountName = builder.Configuration["AzureBlobStorage:AccountName"];

    string accountKey = builder.Configuration["AzureBlobStorage:AccountKey"];

    Uri serviceUri = new($"https://{accountName}.blob.core.windows.net");

    StorageSharedKeyCredential credential = new(accountName, accountKey);

    BlobServiceClient client = new(serviceUri, credential);

    return client;
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
