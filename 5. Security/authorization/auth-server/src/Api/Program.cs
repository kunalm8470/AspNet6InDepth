using Api.Database;
using Api.Middlewares;
using Api.Repositories;
using Api.Services;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Register SqlServerConnectionFactory
builder.Services.AddTransient<ISqlServerConnectionFactory, SqlServerConnectionFactory>(implementationFactory =>
{
    string connectionString = builder.Configuration.GetConnectionString("AuthenticationDatabase");

    return new SqlServerConnectionFactory(connectionString);
});

builder.Services.AddHttpContextAccessor();

// Register Azure Key Vault Service
builder.Services.AddSingleton<IKeyVaultService, KeyVaultService>((s) =>
{
    string vaultUri = builder.Configuration["azureKeyVault:vaultUri"];
    string tenantId = builder.Configuration["azureKeyVault:tenantId"];
    string clientId = builder.Configuration["azureKeyVault:clientId"];
    string clientSecret = builder.Configuration["azureKeyVault:clientSecret"];

    ClientSecretCredential credential = new(tenantId, clientId, clientSecret);

    SecretClient secretClient = new(new Uri(vaultUri), credential);

    return new KeyVaultService(secretClient);
});

// Register account service
builder.Services.AddScoped<IAccountService, AccountService>();

// Register repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddTransient<IRevokedRefreshTokenRepository, RevokedRefreshTokenRepository>();

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
