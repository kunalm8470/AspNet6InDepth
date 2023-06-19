using Azure;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Api.Services;

public class KeyVaultService : IKeyVaultService
{
    private readonly SecretClient _secretClient;

    public KeyVaultService(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public async Task<ECDsaSecurityKey> GetEcdsaPrivateKeyAsync()
    {
        string secretKey = "token-ecdsa-privatekey";

        // Load the ECDSA P521 private key from Azure Key Vault
        Response<KeyVaultSecret> secretBundle = await _secretClient.GetSecretAsync(secretKey);

        string secretValue = secretBundle?.Value?.Value;

        // Strip the markers
        string privateKeyPemData = secretValue
        .Replace("-----BEGIN EC PRIVATE KEY-----", string.Empty)
        .Replace("-----END EC PRIVATE KEY-----", string.Empty)
        .Replace("\n", string.Empty);

        /* 
         * Convert the PEM content into byte array, as all cryptographic operations
         * work on byte[]
        */
        byte[] privateKeyBytes = Convert.FromBase64String(privateKeyPemData);

        ECDsa ecdsa = ECDsa.Create();

        // Import the private key bytes into ECDsa instance
        ecdsa.ImportECPrivateKey(new ReadOnlySpan<byte>(privateKeyBytes), out _);

        ECDsaSecurityKey securityKey = new(ecdsa);

        return securityKey;
    }

    public async Task<ECDsaSecurityKey> GetEcdsaPublicKeyAsync()
    {
        string secretKey = "token-ecdsa-publickey";

        // Load the ECDSA P521 private key from Azure Key Vault
        Response<KeyVaultSecret> secretBundle = await _secretClient.GetSecretAsync(secretKey);

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
}
