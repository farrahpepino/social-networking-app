using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace server.Middlewares{
    public class GlobalExceptionMiddleware{
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware (RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger){
            _next = next;
            _logger = logger;    
        }

        public async Task Invoke(HttpContext context){
            try{
                await _next(context);         
            }
            catch(Exception ex){
                _logger.LogError(ex, "Unhandled exception");
                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    status = context.Response.StatusCode,
                    error = "An unexpected error occurred",
                    detail = ex.Message,
                    timestamp = DateTime.Now
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}