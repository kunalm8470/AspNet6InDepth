using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression((options) =>
{
    // Response compression needed for both HTTP and HTTPS scheme
    options.EnableForHttps = true;

    // Support for Brotli and gzip compression providers
    options.Providers.Add<BrotliCompressionProvider>();

    options.Providers.Add<GzipCompressionProvider>();

    /*
     *  Exclude any mime type which doesn't need to be compressed
    */
    options.ExcludedMimeTypes = new string[]
    {
        "text/plain"
    };
});

builder.Services.Configure<BrotliCompressionProviderOptions>((options) =>
{
    /*
     * CompressionLevel.Fastest -> Compression time will be less and Compression ratio will be less
     * CompressionLevel.Slowest -> Compression time will be more and Compression ratio will be more
     * CompressionLevel.None -> None, give the response as it is
     * CompressionLevel.Optimal -> Both compression time and ratio will be balanced
    */
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>((options) =>
{
    /*
     * CompressionLevel.Fastest -> Compression time will be less and Compression ratio will be less
     * CompressionLevel.Slowest -> Compression time will be more and Compression ratio will be more
     * CompressionLevel.None -> None, give the response as it is
     * CompressionLevel.Optimal -> Both compression time and ratio will be balanced
    */
    options.Level = CompressionLevel.Optimal;
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register response compression middleware
// Register as early as possible
app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
