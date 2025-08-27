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

        // No ei varmaan järjestystä enää.
        // 27/08/2025

        [HttpPost("createrepo")]
        public IActionResult CreateRepository([FromBody] CreateRepoInput cliRepoCreation)
        {
            try
            {
                if (string.IsNullOrEmpty(cliRepoCreation.RepositoryName))
                {
                    return BadRequest("Repository name cannot be empty.");
                }
                if (string.IsNullOrEmpty(cliRepoCreation.Owner))
                {
                    return BadRequest("Owner cannot be empty.");
                }
                RepositoryManager repoManager = new();
                repoManager.CreateRepositoryCore(cliRepoCreation.RepositoryName, cliRepoCreation.Owner, cliRepoCreation.Description);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Creation successful");
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

        public static void DestroySSHKey()
        {

        }
        
        // Mikä tämä on?
        [HttpPost("LogToDB")]
        public IActionResult LogToDB([FromBody] string monkey)
        {
            return Ok(monkey == null ? "monkey" : monkey);
        }
    }
}
