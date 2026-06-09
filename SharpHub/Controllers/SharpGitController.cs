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

                // Mister minä
                //
                // Tähänhän tarvitsee authorized_keys luku ettei mennä samaa avainta tunkemaan.
                // 2 asiaa
                // 1. if authorized_keys exists
                // 2. if string not in authorized_keys
                try
                {
                    string auth_keys_path = "/home/git/.ssh/authorized_keys";
                    string auth_keys = System.IO.File.ReadAllText(auth_keys_path);

                    //NOTE: Tähän jokin joka rakentaa sen SSH key string mikä append authorized_keys
                    // Eli < shell > < KeyiwID > <SSH key in string format>

                    if (!auth_keys.Contains("test"))
                        using (StreamWriter sw = System.IO.File.AppendText(auth_keys_path))
                        {
                            sw.WriteLine();
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Shit's borked" + ex.Message);

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
