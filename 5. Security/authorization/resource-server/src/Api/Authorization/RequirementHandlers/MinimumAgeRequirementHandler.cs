using Api.Authorization.Requirements;
using Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Api.Authorization.RequirementHandlers;

public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MinimumAgeRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        // To find out if "dateOfBirth" claim is present in the accessToken
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            _httpContextAccessor.HttpContext.Response.ContentType = "application/json";

            await _httpContextAccessor.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = "Date of birth claim not found!"
            }));

            await _httpContextAccessor.HttpContext.Response.CompleteAsync();

            context.Fail();

            return;
        }

        string dateOfBirthClaim = context.User.FindFirst(ClaimTypes.DateOfBirth)?.Value;

        DateTime dateOfBirth = Convert.ToDateTime(dateOfBirthClaim);

        int ageInYears = dateOfBirth.CalculateAge();

        if (ageInYears < requirement.MinimumAge)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            _httpContextAccessor.HttpContext.Response.ContentType = "application/json";

            await _httpContextAccessor.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = $"Current age: {ageInYears}, Minimum age to vote: {requirement.MinimumAge}."
            }));

            await _httpContextAccessor.HttpContext.Response.CompleteAsync();

            context.Fail(); // To exit the requirement handler

            return;
        }

        // The user has been authorized.
        context.Succeed(requirement);
    }
}
