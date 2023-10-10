using Api;
using Api.Interfaces;
using Api.Middlewares;
using Api.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Register Npgsql connection
string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddTransient(implementationFactory =>
{
    return new PostgresConnectionFactory(connectionString);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IPersonsRepository, PersonsRepository>();

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

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
