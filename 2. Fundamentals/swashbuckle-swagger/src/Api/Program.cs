using Microsoft.OpenApi.Models;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Persons V1 API",
        Description = "An ASP .NET 6 WEB API for managing persons",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "John Doe Developer",
            Email = "johndoe@mailinator.com",
            Url = new Uri("https://www.johndoe.com")
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Persons V2 API",
        Description = "An ASP .NET 6 WEB API for managing persons",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "John Doe Developer",
            Email = "johndoe@mailinator.com",
            Url = new Uri("https://www.johndoe.com")
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });

    //1. Generate XML and read it
    string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

    options.IncludeXmlComments(xmlFilePath);
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // 2. Define the path of the swagger.json file
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1.0 Docs");

        c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2.0 Docs");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
