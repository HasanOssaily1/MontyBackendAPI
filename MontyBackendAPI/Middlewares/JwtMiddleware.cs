namespace MontyBackendAPI.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using MontyBackendAPI.Classes;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(IConfiguration configuration, RequestDelegate next)
        {
            _configuration = configuration;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            //list of bypassed API'S from Auth
            var pathsToExclude = new List<string>
            {
                "/api/auth"
            };

            if (pathsToExclude.Any(path => context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }
            JwtHelper tokenService = new JwtHelper();
            // Extract the token from the Authorization header
            string authHeader = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            // Validate the token and get the claims principal
            if (!string.IsNullOrEmpty(authHeader) && tokenService.IsTokenValid(_configuration["Jwt:accessKey"].ToString(), _configuration["Jwt:Issuer"].ToString(), authHeader))
            {
                try
                {
                    var principal = tokenService.GetPrincipalFromToken(authHeader, _configuration["Jwt:Issuer"].ToString(), _configuration["Jwt:accessKey"].ToString());

                    // Extract the custom member ID claim
                    var memberIdClaim = principal?.FindFirst("ID")?.Value;

                    if (!string.IsNullOrEmpty(memberIdClaim))
                    {
                        // Add the member ID to the request headers
                        context.Request.Headers["MemberID"] = memberIdClaim;
                    }

                }
                catch
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid token");
                    return;
                }

                // Continue processing the request
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }


}
