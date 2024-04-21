
namespace NascodeCMS.Server.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Request {method} {url} {param} {host}, Agent {agent} => {statusCode}",
                   context.Request?.Method,
                   context.Request?.Path.Value,
                   string.Join("&", (context.Request?.Query)),
                   context.Request?.Host,
                   context.Request?.Headers["User-Agent"].ToString(),
                   context.Response?.StatusCode);
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Request {method} {url} {param} {host}, Agent {agent} => {statusCode} Exception: {Exception}",
                  context.Request?.Method,
                  context.Request?.Path.Value,
                  string.Join("&", (context.Request?.Query)),
                  context.Request?.Host,
                  context.Request?.Headers["User-Agent"].ToString(),
                  context.Response?.StatusCode, ex.Message);
            }
        }
    }

   
}
