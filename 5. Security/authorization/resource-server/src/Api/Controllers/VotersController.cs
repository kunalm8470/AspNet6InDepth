using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VotersController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VotersController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("votingstatus")]
    [Authorize(Policy = "Age18AndAbove")]
    public IActionResult GetVotingStatus()
    {
        string firstName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;

        string lastName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Surname).Value;

        return Ok(new 
        {
            firstName,
            lastName,
            canVote = true
        });
    }
}
