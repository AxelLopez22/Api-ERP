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
        private readonly IHttpContextAccessor _context;

        public LoginServices(IConfiguration config, ITokenServices token, IHttpContextAccessor context)
        {
            _config = config;
            _token = token;
            _context = context;
        }

        public async Task<BaseResponse> Login(UserLoginDTO user)
        {
            var response = new BaseResponse();
            List<UserLoginDTO> listUsers = new List<UserLoginDTO>
            {
                new UserLoginDTO { UserName = "admin", Password = "admin" },
                new UserLoginDTO { UserName = "user", Password = "12345" }
            };

            var usuario = listUsers.Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();

            if (usuario != null)
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

        public async Task<bool> Logout(string token)
        {
            //var token = _context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {

                return true;
            }
            return false;
        }

        public List<SideMenu> ListarMenu(string UserName)
        {
            List<SideMenu> menu = new List<SideMenu>
            {
                new SideMenu
                {
                    routeLink = "dashboard",
                    icon = "fa-solid fa-house",
                    label = "Dashboard"
                },
                new SideMenu
                {
                    //routeLink = "catalogos",
                    icon = "fa-solid fa-layer-group",
                    label = "Catálogos",
                    items = new List<SideMenu>
                    {
                        new SideMenu
                        {
                            routeLink = "catalogos/rutas",
                            icon = "fa-solid fa-route",
                            label = "Rutas"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/ubicaciones",
                            icon = "fa-solid fa-location-dot",
                            label = "Tipos Ubicaciones"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/pasillos",
                            icon = "fa-solid fa-road",
                            label = "Pasillos"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/lados",
                            icon = "fa-solid fa-left-right",
                            label = "Lados"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/nivel",
                            icon = "fa-solid fa-stairs",
                            label = "Nivel"
                        }, 
                        new SideMenu
                        {
                            routeLink = "catalogos/estadoUbicacion",
                            icon = "fa-solid fa-arrows-up-down-left-right",
                            label = "Estado Ubicacion"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/perfiles",
                            icon = "fa-solid fa-users-rectangle",
                            label = "Perfiles"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/turnos",
                            icon = "fa-solid fa-clock",
                            label = "Turnos"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/empleados",
                            icon = "fa-solid fa-users",
                            label = "Empleados"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/gruposTrabajos",
                            icon = "fa-solid fa-people-line",
                            label = "Grupos Trabajos"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/camiones",
                            icon = "fa-solid fa-truck-moving",
                            label = "Camiones"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/camioneros",
                            icon = "fa-solid fa-user-group",
                            label = "Camioneros"
                        },
                        new SideMenu
                        {
                            routeLink = "catalogos/terminalesPicking",
                            icon = "fa-solid fa-boxes-packing",
                            label = "Terminales Picking"
                        }
                    }
                },
                new SideMenu
                {
                    icon = "fa-solid fa-boxes-stacked",
                    label = "Productos",
                    items = new List<SideMenu>
                    {
                        new SideMenu
                        {
                            routeLink = "productos/list",
                            icon = "fa-solid fa-list-ul",
                            label = "Lista Productos"
                        },
                        new SideMenu
                        {
                            routeLink = "productos/create",
                            icon = "fa-solid fa-circle-plus",
                            label = "Crear Productos"
                        }
                    }
                },
                new SideMenu
                {
                    icon = "fa-solid fa-warehouse",
                    label = "Bodegas",
                    items = new List<SideMenu>
                    {
                        new SideMenu
                        {
                            routeLink = "bodegas/list",
                            icon = "fa-solid fa-store",
                            label = "Lista de bodegas"
                        }
                    }
                },
                new SideMenu
                {
                    routeLink = "transportes",
                    icon = "fa-solid fa-truck-moving",
                    label = "Transportes"
                },
                new SideMenu
                {
                    routeLink = "usuarios",
                    icon = "fa-solid fa-users",
                    label = "Usuarios"
                },
                new SideMenu
                {
                    routeLink = "configuraciones",
                    icon = "fa-solid fa-gear",
                    label = "Configuraciones"
                }
            };

            List<SideMenu> menu2 = new List<SideMenu>
            {
                new SideMenu
                {
                    routeLink = "dashboard",
                    icon = "fa-solid fa-house",
                    label = "Dashboard"
                },
                new SideMenu
                {
                    icon = "fa-solid fa-boxes-stacked",
                    label = "Productos",
                    items = new List<SideMenu>
                    {
                        new SideMenu
                        {
                            routeLink = "productos/list",
                            icon = "fa-solid fa-list-ul",
                            label = "Lista Productos"
                        },
                        new SideMenu
                        {
                            routeLink = "productos/create",
                            icon = "fa-solid fa-circle-plus",
                            label = "Crear Productos"
                        }
                    }
                },
                new SideMenu
                {
                    icon = "fa-solid fa-warehouse",
                    label = "Bodegas",
                    items = new List<SideMenu>
                    {
                        new SideMenu
                        {
                            routeLink = "bodegas/list",
                            icon = "fa-solid fa-store",
                            label = "Lista de bodegas"
                        }
                    }
                },
                new SideMenu
                {
                    routeLink = "transportes",
                    icon = "fa-solid fa-truck-moving",
                    label = "Transportes"
                }
            };

            if (UserName == "admin")
            {
                return menu;
            }
            if (UserName == "user")
            {
                return menu2;
            }

            return null;
        }
    }
}
