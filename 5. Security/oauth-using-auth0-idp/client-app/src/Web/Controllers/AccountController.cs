using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("/login", Name = "login")]
    public async Task Login(string returnUrl = "/")
    {
        AuthenticationProperties authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(returnUrl)
        .Build();

        // Redirect to Auth0 IdP's login page
        await _httpContextAccessor.HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

    [Authorize]
    [HttpGet("/logout", Name = "logout")]
    public async Task Logout()
    {
        AuthenticationProperties authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(Url.Action("Index", "Home"))
        .Build();

        await _httpContextAccessor.HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
