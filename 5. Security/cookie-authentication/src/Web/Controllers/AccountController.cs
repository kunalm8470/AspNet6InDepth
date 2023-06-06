using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models;
using Web.Services;

namespace Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(
        IUserService userService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userService = userService;

        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(ModelState);
        }

        (bool isAuthenticated, string loggedInRole) = _userService.AuthenticateUser(loginModel.Email, loginModel.Password);

        // Return back to login page
        if (!isAuthenticated)
        {
            ModelState.AddModelError("", "Invalid credentials");

            return View(ModelState);
        }

        // 1. Generate Claims
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, loginModel.Email),
            new Claim(ClaimTypes.Email, loginModel.Email),
            new Claim(ClaimTypes.Role, loggedInRole)
        };

        // 2. Generate ClaimsIdentity
        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // 3. Generate ClaimsPrincipal
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

        // 4. Generate Authentication metadata via Authentication properties
        AuthenticationProperties authenticationProperties = new()
        {
            IssuedUtc = DateTimeOffset.UtcNow,

            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),

            RedirectUri = loginModel.ReturnUrl
        };

        // 5. Generate a sign-in cookie
        await _httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);

        // 6. Redirect to landing page
        return Redirect(loginModel.ReturnUrl);
    }

    [HttpPost]
    public async Task<ActionResult> Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync();

        return Redirect("/");
    }
}
