using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public interface IKeyVaultService
{
    Task<ECDsaSecurityKey> GetEcdsaPrivateKeyAsync();

    Task<ECDsaSecurityKey> GetEcdsaPublicKeyAsync();
}
