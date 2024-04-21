using Microsoft.IdentityModel.Tokens;
using MontyBackendAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MontyBackendAPI.Classes
{
    public class JwtHelper
    {
        private const double EXPIRY_DURATION_MINUTES = 60; // Token expiry duration set to half a day.

        /// Builds a JWT token for the specified user. 
        /// key The secret key for signing the token 
        /// issuer The issuer of the token 
        /// user The user object for whom the token is being generated
        /// A JWT token string.
        public string BuildToken(string key, string issuer, Users user, double duration)
        {
            var claims = new[] {
                new Claim(type: "ID", value: user.id.ToString() ?? "0"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddMinutes(duration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        ///  True if the token is valid, otherwise false 
        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = issuer,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = mySecurityKey,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false; // Token validation failed
            }
            catch (Exception e)
            {
                // Log the exception details for troubleshooting.
                // LogException(e);
                return false;
            }
            return validatedToken != null; // Return true if the token is valid.
        }


        public ClaimsPrincipal GetPrincipalFromToken(string token, string issuer, string jwtSecret)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return principal;
            }
            catch (SecurityTokenException)
            {
                return null; // Token validation failed
            }
            catch (Exception e)
            {
                // Log the exception details for troubleshooting.
                // LogException(e);
                return null;
            }
        }
    }
}
