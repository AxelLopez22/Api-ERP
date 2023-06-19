using System.ComponentModel.DataAnnotations;

namespace Api_Almacen.Models.DTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "La contrasenia es requerida")]
        public string? Password { get; set; }
    }

    public class ValidateTokenDTO
    {
        public string Token { get; set; }
    }
}
