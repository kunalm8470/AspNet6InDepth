using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Requirement;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    public string Issuer { get; }

    public string Permission { get; }

    public HasPermissionRequirement(string issuer, string permission)
    {
        Issuer = issuer;

        Permission = permission;
    }
}
