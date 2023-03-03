using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CultureController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CultureController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult SetCurrentCulture([FromQuery] string culture)
    {   
        /*
            Add culture in .AspNetCore.Culture cookie
        */
        _httpContextAccessor.HttpContext.Response.Cookies.Append(
            key: CookieRequestCultureProvider.DefaultCookieName,
            value: CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            options: new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            }
        );

        return Ok();
    }
}
