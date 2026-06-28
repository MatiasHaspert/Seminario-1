using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    /// <summary>Estado de habilitación que el administrador aplica a un usuario.</summary>
    public class UpdateUserStatusDTO
    {
        /// <summary><c>true</c> para habilitar la cuenta; <c>false</c> para darla de baja lógicamente.</summary>
        /// <example>false</example>
        [Required]
        public bool IsActive { get; set; }
    }
}
