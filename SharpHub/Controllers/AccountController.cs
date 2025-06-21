using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;
using System.Security.Claims;

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
        public IActionResult Login(UserLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var loginman = new User
                {
                    Username = vm.Username,
                    Password = vm.Password
                };
                var vastaavuus = MongoManipulator.Search(loginman);

                if (vastaavuus != null)
                {
                    // Voi olla joko vm.Password tai loginman.password
                    if (vastaavuus.Password == vm.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, vm.Username),
                            new Claim(ClaimTypes.NameIdentifier, vastaavuus._id.ToString())

                        };
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5)
                        };

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        return RedirectToAction("Index", "Home");
                    }
                    else { ViewData["ErrorMessage"] = "Invalid username or password."; }
                }
                else { ViewData["ErrorMessage"] = "Invalid username or password."; }
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
            }
            var Bigvm = new AccountIndexViewModel
            {
                Login = vm
            };
            return View("Index", Bigvm);
        }
        // Tällä psh komennolla saa kiinni.
        // Invoke-RestMethod -Uri http://localhost:5227/consolelogin -Method POST -Body '"monkey"' -ContentType 'application/json'
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
        public IActionResult Register(UserRegisterViewModel vm)
        {
            // Check if the username already exists
            var existingUser = MongoManipulator.Search(new User { Username = vm.Username });

            if (!ModelState.IsValid || existingUser != null)
            {
                // If the username exists or model is invalid, add an error and return
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username already taken.");
                }

                // Return to the Index view with the Register view model (including any validation errors)
                var BigvmForError = new AccountIndexViewModel
                {
                    Register = vm,
                    Login = new UserLoginViewModel()
                };
                BigvmForError.Register.RegisterStatus = true; // Keeps the register form visible on error
                return View("Index", BigvmForError);
            }

            // If registration is valid, create and save the new user
            var user = new User
            {
                Username = vm.Username,
                Password = vm.Password
            };
            MongoManipulator.Save(user);

            // After successful registration, set RegisterStatus to true to show the login form
            var BigvmForSuccess = new AccountIndexViewModel
            {
                Register = vm,
                Login = new UserLoginViewModel()
            };
            BigvmForSuccess.Register.RegisterStatus = false; // Hide register form and show login
            return View("Index", BigvmForSuccess);
        }
    }
}
