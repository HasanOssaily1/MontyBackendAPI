using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MontyBackendAPI.Classes;
using MontyBackendAPI.Dto;
using MontyBackendAPI.Models;
using MontyBackendAPI.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace MontyBackendAPI.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUsersRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<JWT>> login([FromBody] UserCredentials userCredentials)
        {
            var user = await _usersRepository.GetUserByEmail(userCredentials.email);
            if (user == null)
            {
                return BadRequest("cannot find user");
            }
            var hashedpassword = HashPassword(userCredentials.password);
            if (hashedpassword == user.password)
            {
                JwtHelper jwtHelper = new JwtHelper();
                var accessToken = jwtHelper.BuildToken(_configuration["Jwt:accessKey"].ToString(), _configuration["Jwt:Issuer"].ToString(), user, 1);
                var refreshToken = jwtHelper.BuildToken(_configuration["Jwt:refreshKey"].ToString(), _configuration["Jwt:Issuer"].ToString(), user, 1440);
                return Ok(new JWT(accessToken, refreshToken));
            }

            return BadRequest("cannot find user");
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<JWT>> getnewtokens([FromBody] JWT token)
        {

            JwtHelper jwtHelper = new JwtHelper();
            if(jwtHelper.IsTokenValid(_configuration["Jwt:refreshKey"].ToString(), _configuration["Jwt:Issuer"].ToString(), token.refreshToken))
            {
                var principal = jwtHelper.GetPrincipalFromToken(token.refreshToken, _configuration["Jwt:Issuer"].ToString(), _configuration["Jwt:refreshKey"].ToString());

                // Extract the custom member ID claim
                var userid = principal?.FindFirst("ID")?.Value;

                
                if (userid == null)
                {
                    return BadRequest("User id not found");
                }
                var user = new Users();
                user.id = int.Parse(userid);

                var accessToken = jwtHelper.BuildToken(_configuration["Jwt:accessKey"].ToString(), _configuration["Jwt:Issuer"].ToString(), user, 1);
                var refreshToken = jwtHelper.BuildToken(_configuration["Jwt:refreshKey"].ToString(), _configuration["Jwt:Issuer"].ToString(), user, 1400);
                return Ok(new JWT(accessToken, refreshToken));

            }

            return BadRequest("invalid refresh token");
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute hash of the password bytes
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert hashed bytes to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }



    }
}
