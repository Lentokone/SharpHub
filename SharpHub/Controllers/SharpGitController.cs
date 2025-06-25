using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;

namespace SharpHub.Controllers
{
    [RequireHttps]
    [Route("api/cli/auth")]
    public class SharpGitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //   http://localhost:5227/api/cli/auth/consolelogin

        // Tässä parempi komento testata
        // curl -H "Content-Type: application/json" -d "monkey" https://localhost:7173/api/cli/auth/consolelogin
        // Powershit käyttääkin invoke-restmethod jolla on vaan "curl" alias jostain syystä

        // Tällä psh komennolla saa kiinni.
        // Invoke-RestMethod -Uri http://localhost:5227/consolelogin -Method POST -Body '"monkey"' -ContentType 'application/json'
        // Tähän napataan se RateLimit Nuugetti paketti.
        // Ja annettu on model jossa on user credentials ja sitten super secret string, joka tarkistetaan.
        [HttpPost("consolelogin")]
        public IActionResult ConsoleLogin([FromBody] UserLoginForCLI loginkontsat)
        {
            string monkey = "monkey balls";

            if (loginkontsat != null)
            {
                //return BadRequest("Invalid login credentials.");

                var clilogin = new User
                {
                    Username = loginkontsat.Username,
                    Password = loginkontsat.Password
                };

            }
            return Ok(monkey);
        }

        // Ja sitten se funktio, joka tekee sen JWT generation
        // Funktio joka tekee sen SSH key generoinnin per user per repo.
        // Funktio joka tuhoaa sen SSH keyn.

        // Ja funktio joka palauttaa sen JWT tokenin ja SSH keyn.

        // Myös tarvitsee sen funktion joka vastaanottaa jonkun post messagen---
        // kun käyttäjä push:aa niin se logaa tietokantaan jtn vastaavaa, "user: Uid, repo: RepoName, action: push, commit: hash, time: 2023-10-01T12:00:00Z"
    }
}
