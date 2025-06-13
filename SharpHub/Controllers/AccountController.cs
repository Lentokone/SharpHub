using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SharpHub.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult _Login()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login()
        {
            return View();
        }

        // Tähän napataan se RateLimit Nuugetti paketti.
        // Ja annettu on model jossa on user credentials ja sitten super secret string, joka tarkistetaan.
        [HttpPost("consolelogin")]
        public IActionResult ConsoleLogin([FromBody] String loginkontsat)
        {
            string monkey = "monkey";
            if (string.IsNullOrEmpty(loginkontsat) || loginkontsat != "monkey")
            {
                return BadRequest("Invalid login credentials.");
            }
            return Ok(monkey);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult _Register()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register()
        {
            return View();
        }
    }
}
