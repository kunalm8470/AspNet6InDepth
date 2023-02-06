using Api;
using Api.Interfaces;
using Api.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add support for handling Content-Type: "application/json-patch+json"
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddTransient<SqlServerConnectionFactory>((serviceProvider) =>
{
    string connectionString = builder.Configuration.GetConnectionString("Default");

    return new SqlServerConnectionFactory(connectionString);
});

builder.Services.AddTransient<IPersonRepository, PersonRepository>();

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

app.Run();
