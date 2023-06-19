using Api_Almacen.Models.DTOs;
using Api_Almacen.Models.Response;
using Api_Almacen.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Almacen.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _config;

        public TokenServices(IConfiguration config)
        {
            _config = config;
        }

        public async Task<AuthResponse> GenerateToken(UserLoginDTO user)
        {
            var Claims = new List<Claim>()
            {
                new Claim("Usuario", user.UserName)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["LlaveJwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var Expiracion = DateTime.UtcNow.AddHours(8);
            var securityToken = new JwtSecurityToken(claims: Claims,
                expires: Expiracion, signingCredentials: creds);

            return new AuthResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken)
            };
        }

        public async Task<BaseResponse> ValidateToken(string token)
        {
            var response = new BaseResponse();
            if(token == null)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = Messages.MESSAGE_FAILED_AUTH;
                return response;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_config["LlaveJwt"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["LlaveJwt"])),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var user = jwtToken.Claims.First(x => x.Type == "Usuario").Value;

                response.IsSuccess = true;
                response.Data = user;
                response.Message = Messages.MESSAGE_SUCCESS_VALIDATE_TOKEN;

                return response;
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = Messages.MESSAGE_FAILED_VALIDATE_TOKEN;
                return response;
            }

        }
    }

    public interface ITokenServices
    {
        public Task<AuthResponse> GenerateToken(UserLoginDTO user);
        public Task<BaseResponse> ValidateToken(string token); 
    }
}
