using FluentValidation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddNewtonsoftJson();

/*
 * Scan the assemblies via reflection and give entry point
 * Entry point here is Program.cs
 * This helper method is found in FluentValidation.DependencyInjectionExtensions nuget package
*/
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Transient);

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
