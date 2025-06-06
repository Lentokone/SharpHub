using Microsoft.AspNetCore.Mvc;

namespace SharpHub.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
