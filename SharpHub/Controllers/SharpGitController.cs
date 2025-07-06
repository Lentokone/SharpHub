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
        private readonly JwtTokenService _jwtService;

        public SharpGitController(JwtTokenService jwtService)
        {
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
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
                    var token = _jwtService.GenerateJWTToken(vastaavuus.Username, cliLoginContent.RepositoryName, clilogin.Password);

                }
                else { monkey = "Joku ei toiminut"; }
                //monkey = clilogin.Username.ToString();

                // Note to self:
                // Tämä tulee palauttamaan JWT token, sille refresh token, ja SSH keyn.
                
                return Ok(monkey);
            }
            else
            {
                return Ok(monkey);
            }
        }

        // Placeholder funktio, joka logaa refresh ja JWT, tietokantaan.
        public static void LogJWTRefresh(string username, string jwtToken, string refreshToken)
        {
            // Tähän loggaus tietokantaan.
            // MongoManipulator.Save(new JWTLog { Username = username, JwtToken = jwtToken, RefreshToken = refreshToken });
        }

        /// Tosijaan kommenttia taas tänne::::
        //! 04/07/2025
        // Olisi kiva logata tietokantaan, Failed login, Commit pushed, Repo created / deleted, Unhandled error logged.


        //[HttpPost("testing")]
        //public IActionResult JwtGenerationTest()
        //{
        //    var token = _jwtService.GenerateJWTToken(new User { Username = "testuser" });
        //    return Ok();
        //}

        public static void DestroySSHKey()
        {

        }

        // Ja sitten se funktio, joka tekee sen JWT generation
        // Funktio joka tekee sen SSH key generoinnin per user per repo.
        // Funktio joka tuhoaa sen SSH keyn.

        // Ja funktio joka palauttaa sen JWT tokenin ja SSH keyn.

        // Myös tarvitsee sen funktion joka vastaanottaa jonkun post messagen---
        // kun käyttäjä push:aa niin se logaa tietokantaan jtn vastaavaa, "user: Uid, repo: RepoName, action: push, commit: hash, time: 2023-10-01T12:00:00Z"
        [HttpPost("LogToDB")]
        public IActionResult LogToDB([FromBody] string monkey)
        {
            return Ok(monkey == null ? "monkey" : monkey);
        }
    }
}
