using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace SharpHub.Controllers
{
    public partial class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPartial()
        {
            return PartialView("_Login");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginViewModel vm)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(vm.Username) && !string.IsNullOrEmpty(vm.Password))
            {
                var loginman = new User
                {
                    Username = vm.Username,
                    Password = vm.Password
                };
                var vastaavuus = MongoManipulator.Search(loginman);

                if (vastaavuus != null)
                {
                    if (vastaavuus.Password == vm.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Name, vm.Username),
                            new(ClaimTypes.NameIdentifier, vastaavuus._id.ToString())

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
                Login = vm,
            };
            return View("Index", Bigvm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RegisterPartial()
        {
            return PartialView("_Register");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterViewModel vm)
        {
            vm.Username = vm.Username.Trim();
            if (!UsernameRegex().IsMatch(vm.Username))
            {
                ModelState.AddModelError("Username", "Username contains characters that are not allowed.");
            }
            var responseVm = new AccountIndexViewModel
            {
                Register = vm,
                Login = new UserLoginViewModel()
            };

            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(vm.Username))
                {
                    var existingUser = MongoManipulator.Search(new User { Username = vm.Username });
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Username", "Username already taken.");
                    }
                }

                vm.RegisterStatus = false; // Keep register form visible
                return View("Index", responseVm);
            }

            var duplicate = MongoManipulator.Search(new User { Username = vm.Username });
            if (duplicate != null)
            {
                ModelState.AddModelError("Username", "Username already taken.");
                vm.RegisterStatus = false;
                return View("Index", responseVm);
            }

            MongoManipulator.Save(new User
            {
                Username = vm.Username,
                Password = vm.Password
            });

            responseVm.Register.RegisterStatus = true;
            return View("Index", responseVm);
        }

        [GeneratedRegex("^[a-zA-Z0-9_-]{1,100}$")]
        private static partial Regex UsernameRegex();
    }
}
