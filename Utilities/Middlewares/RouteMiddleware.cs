using Api_Almacen.Models.Response;
using Api_Almacen.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Almacen.Utilities.Middlewares
{
    public class RouteMiddleware
    {
        private readonly RequestDelegate _next;

        public RouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await validateAccess(context, token);   
            }

            await _next(context);
        }

        public async Task<bool> validateAccess(HttpContext context, string token)
        {
            //var response = new BaseResponse();
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var decodedToken = jwtHandler.ReadJwtToken(token);

                IEnumerable<Claim> claims = decodedToken.Claims;

                foreach (Claim claim in claims)
                {
                    string claimType = claim.Type;
                    string claimValue = claim.Value;

                    if(claimType == "Usuario" && claimValue == "admin")
                    {
                        context.Items["EsAdmin"] = new User
                        {
                            Username = claimValue
                        };
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
