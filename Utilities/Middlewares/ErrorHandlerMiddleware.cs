using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Api_Almacen.Utilities.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;

                if (!response.HasStarted)
                {
                    response.ContentType = "application/json";

                    switch (error)
                    {
                        case AppException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case KeyNotFoundException e:
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        default:
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }

                    _logger.LogError(error, error.Message, error.Data);
                    var result = JsonSerializer.Serialize(new { 
                        statusCode = response.StatusCode, 
                        message = error?.Message,
                        details = error?.StackTrace
                    });
                    await response.WriteAsync(result);
                }
            }
        }
    }
}
