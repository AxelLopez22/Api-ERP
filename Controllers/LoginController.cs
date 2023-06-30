using Api_Almacen.Models.DTOs;
using Api_Almacen.Models.Response;
using Api_Almacen.Services;
using Api_Almacen.Utilities;
using Api_Almacen.Utilities.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;


namespace Api_Almacen.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LoginServices _services;
        private readonly ILogger<LoginController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginController(IConfiguration config, ITokenServices tokenServices, ILogger<LoginController> logger, IHttpContextAccessor contextAccessor)
        {
            _services = new LoginServices(config, tokenServices, contextAccessor);
            _config = config;
            _logger = logger;
            _contextAccessor = contextAccessor;
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string token)
        {
            var result = await _services.Logout(token);
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

        [HttpGet("estructura")]
        public async Task<IActionResult> CargarEstructura(string userName)
        {
            var response = new BaseResponse();
            response.IsSuccess = true;
            response.Data = _services.ListarMenu(userName);
            response.Message = Messages.MESSAGE_QUERY;

            return Ok(response);
        }

        [AuthorizeAdmin]
        [HttpGet("mensajeAdmin")]
        public IActionResult GetInfoAdmin()
        {
            var response = new BaseResponse();
            response.IsSuccess = true;
            response.Data = null;
            response.Message = Messages.MESSAGE_QUERY;

            return Ok(response);
        }

        //[AllowAnonymous]
        //[HttpPost("prueba")]
        //public IActionResult AddMessage(List<BaseResponse> model)
        //{
        //    throw new ArgumentNullException();
        //    return Ok(model);
        //}
    }
}
