using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ValuesController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}
