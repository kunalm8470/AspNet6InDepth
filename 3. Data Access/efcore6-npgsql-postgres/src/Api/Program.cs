using Api.Data;
using Api.Data.Repositories;
using Api.Interfaces;
using Api.Middlewares;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<CompanyDbContext>(options => options.UseNpgsql(connectionString));

// Access HTTPContext
builder.Services.AddHttpContextAccessor();

// Register the repositories as scoped dependency
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Cors_DevEnvLocal_Policy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Cors_DevEnvLocal_Policy");
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
