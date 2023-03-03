using Microsoft.AspNetCore.Localization;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLocalization(options =>
{
    // Search for resource files in resources folder
    options.ResourcesPath = "Resources";
});

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(options =>
{
    List<CultureInfo> cultures = new()
    {
        new CultureInfo("en"),
        new CultureInfo("fr"),
        new CultureInfo("de"),
    };

    // Console, API and MVC application
    options.SupportedCultures = cultures;

    // MVC application
    options.SupportedUICultures = cultures;

    // Fallback mechanism
    // If culture specified is not in supported culture list
    // it will fallback to this.
    //
    // or if the culture is not passed altogether
    options.DefaultRequestCulture = new RequestCulture("en");

    // Set the Content-Language response header to specify the current culture
    options.ApplyCurrentCultureToResponseHeaders = true;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
