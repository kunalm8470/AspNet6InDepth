#region Register the services (aka ConfigureServies)


using Api.Interfaces;
using Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Service registration
builder.Services.AddSingleton<ISingletonOperation, OperationService>();  // InstancePerLifetime Autofac
builder.Services.AddScoped<IScopedOperation, OperationService>();  // InstancePerRequest Autofac
builder.Services.AddTransient<ITransitiveOperation, OperationService>(); // InstancePerDependency Autofac

builder.Services.AddScoped<IDummyService, DummyService>();

var app = builder.Build();
#endregion


#region Middlewares (aka Configure)

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

#endregion