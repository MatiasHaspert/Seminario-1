using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    /// <summary>Credenciales que envía el usuario para iniciar sesión.</summary>
    public class UserLoginDTO
    {
        /// <summary>Email de la cuenta.</summary>
        /// <example>user1@example.com</example>
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Contraseña de la cuenta.</summary>
        /// <example>user1</example>
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }
}
