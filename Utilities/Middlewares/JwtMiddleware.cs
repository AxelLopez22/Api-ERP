using Api_Almacen.Models.Response;
using Api_Almacen.Services;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace Api_Almacen.Utilities.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration config, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _config = config;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context, ITokenServices tokenServices)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await AttachUserToContext(context, token);
            }
            
            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            var response = new BaseResponse();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["LlaveJwt"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                context.Items["User"] = new User
                {
                    Username = jwtToken.Claims.First(x => x.Type == "Usuario").Value
                };
            }
            catch
            {
                var result = new
                {
                    StatusCode = 401,
                    Message = "Acceso no autorizado"
                };

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result), Encoding.UTF8);
                return;
            }
        }
    }

    internal class User
    {
        public string Username { get; set; }
    }

    internal class BoxedValue<T>
    {
        public T Value { get; }

        public BoxedValue(T value)
        {
            Value = value;
        }
    }
}
