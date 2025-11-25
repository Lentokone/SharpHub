using Microsoft.AspNetCore.Mvc;
using SharpHub.Models;
using SharpHub.Models.Services;

namespace SharpHub.Controllers
{
    [Route("api/ass")]
    [ApiController]
    public class TokenTestController : Controller
    {
        private readonly JwtTokenService _jwtService;
        public TokenTestController(JwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }


        [HttpGet("testing")]
        public IActionResult JwtGenerationTest()
        {
            string monkey1 = "monkey balls1";
            string monkey2 = "monkey balls2";
            string monkey3 = "monkey balls3";
            var token = _jwtService.GenerateJWTToken(monkey1, monkey2, monkey3);
            return Ok(new { token });
            //return Ok("new { token }");
        }
    }
}

// NONNI
// TESTI TOIMII.
/*
 * $ curl https://localhost:7173/api/ass/testing -k
 *  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
 *                                 Dload  Upload   Total   Spent    Left  Speed
 *100   319    0   319    0     0  34445      0 --:--:-- --:--:-- --:--:-- 35444{"token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb25rZXkgYmFsbHMxIiwidW5pcXVlX25hbWUiOiJtb25rZXkgYmFsbHMyIiwicmVwb0lkIjoibW9ua2V5IGJhbGxzMyIsIm5iZiI6MTc1MTY3MDAxNSwiZXhwIjoxNzUxNjg4MDE1LCJpYXQiOjE3NTE2NzAwMTUsImlzcyI6IlNoYXJwSHViIiwiYXVkIjoiU2hhcnBHaXRDbGllbnQifQ.7x9kKRekeWpY4RhqSaZUy0TiQ82RhB00xrEjNxtd17E"}

 */

/*jwt.io
 * decoding.
 * {
  "sub": "monkey balls1",
  "unique_name": "monkey balls2",
  "repoId": "monkey balls3",
  "nbf": 1751670015,
  "exp": 1751688015,
  "iat": 1751670015,
  "iss": "SharpHub",
  "aud": "SharpGitClient"
}
 */
