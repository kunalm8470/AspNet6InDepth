using Api.Authorization.Requirement;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Api.Authorization.RequirementHandler;

public class PermissionRequirementHandler : AuthorizationHandler<HasPermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
    {
        // Check if permissions claim is present or not ?
        // If not present return HTTP 403 (Forbidden)
        if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            _httpContextAccessor.HttpContext.Response.ContentType = "application/json";

            await _httpContextAccessor.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = "Permission claim is not found"
            }));

            await _httpContextAccessor.HttpContext.Response.CompleteAsync();

            // Fail the requirement
            context.Fail();

            return;
        }

        string[] permissions = context.User.FindAll(c => c.Type == "permissions").Select(c => c.Value).ToArray();

        // Permissions coming from requirement are those present in access token or not ?
        if (!permissions.Contains(requirement.Permission))
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            _httpContextAccessor.HttpContext.Response.ContentType = "application/json";

            await _httpContextAccessor.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = "Insufficient Permissions"
            }));

            await _httpContextAccessor.HttpContext.Response.CompleteAsync();

            // Fail the requirement
            context.Fail();

            return;
        }

        // If control came here, all checks are passed
        context.Succeed(requirement);
    }
}
