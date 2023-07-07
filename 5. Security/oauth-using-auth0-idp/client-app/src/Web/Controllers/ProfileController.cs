using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Web.Models;

namespace Web.Controllers;

public class ProfileController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileController(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    public IActionResult Index()
    {
        ClaimsPrincipal currentLoggedInUser = _httpContextAccessor.HttpContext.User;

        Profile profile = new()
        {
            Username = currentLoggedInUser.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value,
            Email = currentLoggedInUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            ProfileImageUrl = currentLoggedInUser.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
        };

        ViewData["serializedUserProfile"] = JsonConvert.SerializeObject(currentLoggedInUser.Claims.Select(x => new
        {
            name = x.Type,
            value = x.Value
        }), Formatting.Indented);

        return View(profile);
    }
}
