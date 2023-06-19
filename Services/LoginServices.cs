using Api_Almacen.Models.DTOs;
using Api_Almacen.Models.Response;
using Api_Almacen.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Almacen.Services
{
    public class LoginServices
    {
        private readonly IConfiguration _config;
        private readonly ITokenServices _token;

        public LoginServices(IConfiguration config, ITokenServices token)
        {
            _config = config;
            _token = token;
        }

        public async Task<BaseResponse> Login(UserLoginDTO user)
        {
            var response = new BaseResponse();
            if (user.UserName == "admin" && user.Password == "admin")
            {
                response.IsSuccess = true;
                response.Data = await _token.GenerateToken(user);
                response.Message = Messages.MESSAGE_TOKEN;
                return response; 
            } else
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = Messages.MESSAGE_FAILED_LOGIN;
                return response;
            }
        }

        public async Task<BaseResponse> Authenticate(ValidateTokenDTO token)
        {
            var result = await _token.ValidateToken(token.Token);
            return result;
        }
    }
}
