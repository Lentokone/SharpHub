using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;
using System;
using System.IO;

namespace SharpHub.Controllers
{
    // [RequireHttps]
    [Route("api/cli/auth")]
    public class SharpGitController : Controller
    {
        // Tässä parempi komento testata
        // curl -H "Content-Type: application/json" -d "monkey" https://localhost:7173/api/cli/auth/consolelogin
        // Powershit käyttääkin invoke-restmethod jolla on vaan "curl" alias jostain syystä

        // Ajan 22.11. 0:34 Olli
        // Eli tämä on se endpoint johon SharpGit lähettää kirjautumistiedot
        // Ja palauttaa refresh token.
        //
        // Ajan 19.05. 14:06 Olli
        // Jotain


        // Refactor this whole function
        //
        // Things it will need:
        // Ok logic
        // SSH key storing
        // Proper login check
        // Proper returns
        [HttpPost("login")]
        public IActionResult ConsoleLogin([FromBody] UserLoginForCLI cliLoginContent)
        {
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
                    return Unauthorized("Invalid credentials.");
                }

                //NOTE:
                // Tähän SSH avain tallennus tietokantaan
                // Sen ObjectId otetaan ja laitetaan se siihen authorized_keys kirjoitukseen

                Console.WriteLine(cliLoginContent.SSHKey);
                Console.WriteLine(Directory.Exists("/home/welho/.ssh"));
                using (StreamWriter sw = System.IO.File.AppendText("/home/git/.ssh/authorized_keys"))
                {
                    sw.WriteLine();
                }
                return Ok("Login successful");
            }
            else
            {
                return BadRequest("Bad input");
            }
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
