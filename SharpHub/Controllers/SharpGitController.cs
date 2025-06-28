using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;
using System;
using System.Runtime.ConstrainedExecution;

namespace SharpHub.Controllers
{
    [RequireHttps]
    [Route("api/cli/auth")]
    public class SharpGitController : Controller
    {
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
            // Testaus string
            string monkey = "monkey balls";

            if (loginkontsat != null)
            {
                var clilogin = new User
                {
                    Username = loginkontsat.Username,
                    Password = loginkontsat.Password
                };
                var vastaavuus = MongoManipulator.Search(clilogin);
                if (vastaavuus == null || vastaavuus.Password != loginkontsat.Password)
                {
                    return BadRequest("Invalid login credentials.");
                }
                if (vastaavuus != null)
                {
                    monkey = $"User {vastaavuus.Username} authenticated successfully.";
                }
                else { monkey = "Joku ei toiminut"; }
                    //monkey = clilogin.Username.ToString();
                    return Ok(monkey);
            }
            else
            {
                return Ok(monkey);
            }
        }
        //28/06/2025
        // Eli tuo saisi palauttaa sen JWT tokenin ja SSH keyn.


        // Nonni. 27/06/2025 2:05 Tuo nyt jotenkin voi vastaanottaa User creds
        //
        // Tällä komennolla
        // $ curl -H "Content-Type: application/json" -d '{ "Username" : "testi1", "Password" : "123" }' https://localhost:7173/api/cli/auth/consolelogin
            //% Total    % Received % Xferd Average Speed Time    Time Time  Current
            //                                Dload  Upload Total   Spent Left  Speed
            //100    84    0    39  100    45   4549   5249 --:--:-- --:--:-- --:--:-- 10500User testi1 authenticated successfully.


        // Ja sitten se funktio, joka tekee sen JWT generation
        // Funktio joka tekee sen SSH key generoinnin per user per repo.
        // Funktio joka tuhoaa sen SSH keyn.

        // Ja funktio joka palauttaa sen JWT tokenin ja SSH keyn.

        // Myös tarvitsee sen funktion joka vastaanottaa jonkun post messagen---
        // kun käyttäjä push:aa niin se logaa tietokantaan jtn vastaavaa, "user: Uid, repo: RepoName, action: push, commit: hash, time: 2023-10-01T12:00:00Z"
    }
}
