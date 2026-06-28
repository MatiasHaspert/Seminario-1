using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    /// <summary>Datos que envía el usuario al registrarse en el sistema.</summary>
    public class UserRegisterDTO
    {
        /// <summary>Nombre del usuario.</summary>
        /// <example>Juan</example>
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Apellido del usuario.</summary>
        /// <example>Pérez</example>
        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>Email único de la cuenta.</summary>
        /// <example>juan.perez@example.com</example>
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Contraseña (entre 6 y 100 caracteres).</summary>
        /// <example>secret123</example>
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        /// <summary>Teléfono de contacto (opcional).</summary>
        /// <example>3415551234</example>
        [MaxLength(20)]
        public string? Phone { get; set; }

        /// <summary>Rol con el que se registra el usuario. 0 = User (huésped), 1 = Owner (dueño).</summary>
        /// <example>0</example>
        [Required(ErrorMessage = "El rol es requerido")]
        public Role role { get; set; } = Role.User;
    }
}
