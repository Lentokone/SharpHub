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
        // No ei varmaan järjestystä enää.
        // 27/08/2025


        //   http://localhost:5227/api/cli/auth/consolelogin

        // Tässä parempi komento testata
        // curl -H "Content-Type: application/json" -d "monkey" https://localhost:7173/api/cli/auth/consolelogin
        // Powershit käyttääkin invoke-restmethod jolla on vaan "curl" alias jostain syystä

        // Tällä psh komennolla saa kiinni.
        // Invoke-RestMethod -Uri http://localhost:5227/consolelogin -Method POST -Body '"monkey"' -ContentType 'application/json'
        // Tähän napataan se RateLimit Nuugetti paketti.
        // Ja annettu on model jossa on user credentials ja sitten super secret string, joka tarkistetaan.

        // Ajan 22.11. 0:34 Olli
        // Eli tämä on se endpoint johon SharpGit lähettää kirjautumistiedot
        // Ja palauttaa refresh token.
        [HttpPost("consolelogin")]
        public IActionResult ConsoleLogin([FromBody] UserLoginForCLI cliLoginContent)
        {
            // Testaus string
            string monkey = "monkey balls";

            if (cliLoginContent != null)
            {
                var clilogin = new User
                {
                    Username = cliLoginContent.Username,
                    Password = cliLoginContent.Password
                };
                var vastaavuus = MongoManipulator.Search(clilogin);
                if (vastaavuus == null || vastaavuus.Password != cliLoginContent.Password)
                {
                    return BadRequest("Invalid login credentials.");
                }
                if (vastaavuus != null)
                {
                    // Tähän tarkistus onko se repo tämän käyttäjän.
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

        public static void DestroySSHKey()
        {

        }

        // Ancient code

        // Mikä tämä on?
        [HttpPost("LogToDB")]
        public IActionResult LogToDB([FromBody] string monkey)
        {
            return Ok(monkey == null ? "monkey" : monkey);
        }
    }
}
