using Api.Authorization.Requirement;
using Api.Authorization.RequirementHandler;
using Api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];

    options.Audience = builder.Configuration["Auth0:Audience"];

    // If token is present, validate the token claims
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Honour only RS256 algorithm and reject others
        ValidAlgorithms = new[] { "RS256" },

        SaveSigninToken = true,

        // Check if issuer is matching or not
        ValidIssuer = builder.Configuration["Auth0:Domain"],
        ValidateIssuer = true,

        // Check if audience is matching or not
        ValidAudience = builder.Configuration["Auth0:Audience"],
        ValidateAudience = true,

        // Check the "exp" claim to check if token is expired or not
        // By default ClockSkew is set to TimeSpan.FromMinutes(5)
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:notes", policy =>
    {
        policy.Requirements.Add(new HasPermissionRequirement(builder.Configuration["Auth0:Domain"], "read:notes"));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

builder.Services.AddSingleton<INoteService, NoteService>();

WebApplication app = builder.Build();

// Middleware to check if authorization header is set or not
app.Use(async (context, next) =>
{
    string authorizationHeader = context.Request.Headers["Authorization"];

    if (string.IsNullOrEmpty(authorizationHeader) || string.IsNullOrWhiteSpace(authorizationHeader))
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsync(
            JsonConvert.SerializeObject(new 
            { 
                message = "No authorization header" 
            })
        );

        await context.Response.CompleteAsync();
    }
    else
    {
        await next.Invoke();
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
