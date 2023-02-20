using Api.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // To add a filter globally
    options.Filters.Add<GlobalActionFilter>();

    // 1. To do constructor injection, add as a service
    options.Filters.AddService<EnsureNonEmptyGuidAsyncActionFilter>();
});

// 2. Also define the lifetime for the filter to do Dependency injection
builder.Services.AddScoped<EnsureNonEmptyGuidAsyncActionFilter>();

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
