using Api.Interfaces;
using Api.Middlewares;
using Api.Services;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDummyService, DummyService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// builder.Services.AddSingleton<FactoryMiddleware>();

builder.Services.AddCors(options =>
{

    options.AddPolicy("Dev_Environment_Policy", builder =>
    {
        builder
        .AllowAnyOrigin() // Access-Control-Allow-Origin: *
        .AllowAnyHeader() // Access-Control-Allow-Headers: *
        .AllowAnyMethod(); // For allowing unsafe requests (any request which is not HTTP GET or HTTP POST) Access-Control-Allow-Method: *
    });

    options.AddPolicy("Prod_Environment_Policy", builder =>
    {
        builder
        .WithOrigins(origins: "https://www.google.com") // production hostname instead of google.com
        .WithMethods("GET", "POST", "PUT", "DELETE");
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Inline middleware


// app.Map is used to map an endpoint to middleware
//app.Map("/test-middleware", (app) =>
//{
//    // app.Run terminates the flow
//    app.Run(async (context) =>
//    {
//        await context.Response.WriteAsJsonAsync(new
//        {
//            message = "Hello world from map middleware"
//        });
//    });
//});

//// app.Use gives control to the next middleware
//app.Use(async (context, next) =>
//{
//    app.Logger.LogInformation("Before first middleware");

//    await next(); // await next.Invoke();

//    app.Logger.LogInformation("Exiting first middleware");
//});

//app.Use(async (context, next) =>
//{
//    app.Logger.LogInformation("Before second middleware");

//    await next(); // await next.Invoke();

//    app.Logger.LogInformation("Exiting second middleware");
//});

//// app.Run terminates the flow
//app.Run(async (context) =>
//{
//    app.Logger.LogInformation("In the terminate middleware");

//    await context.Response.WriteAsJsonAsync(new
//    {
//        message = "Hello world"
//    });
//});

// app.UseMiddleware<ConventionalMiddleware>();

// app.UseMiddleware<FactoryMiddleware>();

/*
    Optionally attach the CORS middleware
    only in dev environment

    This is working by checking ASPNETCORE_ENVIRONMENT variable
*/
if (app.Environment.IsDevelopment())
{
    app.UseCors("Dev_Environment_Policy");
}

if (app.Environment.IsProduction())
{
    app.UseCors("Prod_Environment_Policy");
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<UnhandledExceptionMiddleware>();

/*
    Access the contents of wwwroot folder publicly
*/
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
