using Api_Almacen.Models.DTOs;
using Api_Almacen.Models.Response;
using Api_Almacen.Services;
using Api_Almacen.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Almacen.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LoginServices _services;

        public LoginController(IConfiguration config, ITokenServices tokenServices)
        {
            _services = new LoginServices(config, tokenServices);
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            var result = await _services.Login(user);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(ValidateTokenDTO token)
        {
            var result = await _services.Authenticate(token);
            return Ok(result);
        }

        [HttpGet("mensaje")]
        public IActionResult GetInfo()
        {
            var response = new BaseResponse();
            response.IsSuccess = true;
            response.Data = null;
            response.Message = Messages.MESSAGE_QUERY;
            
            return Ok(response);
        }
    }
}
