using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Requirements;

// To make a class a policy requirment we need to implement IAuthorizationRequirement interface
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

	public MinimumAgeRequirement(int minimumAge)
	{
		MinimumAge = minimumAge;
	}
}
