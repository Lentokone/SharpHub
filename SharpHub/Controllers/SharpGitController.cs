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

                var sshKey = new SSHKey
                {
                    UserID = vastaavuus._id,
                    PublicKey = cliLoginContent.SSHKey,
                };

                try
                {
                    string auth_keys_path = "/home/git/.ssh/authorized_keys";
                    string auth_keys = System.IO.File.ReadAllText(auth_keys_path);

                    string SSHkeyLine = $"command=\"/usr/local/bin/sharphub-shell keyId={sshKey._id}\",no-port-forwarding,no-agent-forwarding,no-X11-forwarding,no-pty {cliLoginContent.SSHKey}";
                    if (!auth_keys.Contains(SSHkeyLine))
                        using (StreamWriter sw = System.IO.File.AppendText(auth_keys_path))
                        {
                            sw.WriteLine(SSHkeyLine);
                        }
                }
                catch (Exception ex)
                {
                    return BadRequest("Writing key into authorized_keys failed" + ex.Message);
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
