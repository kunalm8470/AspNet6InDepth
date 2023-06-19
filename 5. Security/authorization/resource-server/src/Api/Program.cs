using Api.Authorization.RequirementHandlers;
using Api.Authorization.Requirements;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Get ECDSA P-521 public key from Azure Key Vault to validate the access token
ECDsaSecurityKey GetEcdsaKeyFromAzureKeyVault()
{
    string vaultUri = builder.Configuration["azureKeyVault:vaultUri"];
    string tenantId = builder.Configuration["azureKeyVault:tenantId"];
    string clientId = builder.Configuration["azureKeyVault:clientId"];
    string clientSecret = builder.Configuration["azureKeyVault:clientSecret"];

    ClientSecretCredential credential = new(tenantId, clientId, clientSecret);

    SecretClient secretClient = new(new Uri(vaultUri), credential);

    string secretKey = "token-ecdsa-publickey";

    // Load the ECDSA P521 private key from Azure Key Vault
    Response<KeyVaultSecret> secretBundle = secretClient.GetSecret(secretKey);

    string secretValue = secretBundle?.Value?.Value;

    // Strip the markers
    string publicKeyPemData = secretValue
    .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
    .Replace("-----END PUBLIC KEY-----", string.Empty)
    .Replace("\n", string.Empty);

    /* 
     * Convert the PEM content into byte array, as all cryptographic operations
     * work on byte[]
    */
    byte[] keyBytes = Convert.FromBase64String(publicKeyPemData);

    ECDsa ecdsa = ECDsa.Create();

    // Import the public key bytes into ECDsa instance
    ecdsa.ImportSubjectPublicKeyInfo(new ReadOnlySpan<byte>(keyBytes), out _);

    ECDsaSecurityKey securityKey = new(ecdsa);

    return securityKey;
}

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer((options) =>
{
    options.TokenValidationParameters = new()
    {
        ValidIssuer = builder.Configuration["authentication:issuer"],
        ValidateIssuer = true,

        ValidAudience = builder.Configuration["authentication:audience"],
        ValidateAudience = true,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, // No grace period. Expire token at absolute time.
        IssuerSigningKey = GetEcdsaKeyFromAzureKeyVault()
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Age18AndAbove", policy =>
    {
        int minimumAge = builder.Configuration.GetValue<int>("authorization:minimumAge");

        policy.Requirements.Add(new MinimumAgeRequirement(minimumAge));
    });
});

// Register the requirement handler
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeRequirementHandler>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
