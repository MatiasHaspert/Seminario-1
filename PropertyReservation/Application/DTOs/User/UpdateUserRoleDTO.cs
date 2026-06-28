using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.User
{
    /// <summary>Nuevo rol que el administrador asigna a un usuario.</summary>
    public class UpdateUserRoleDTO
    {
        /// <summary>Rol destino. 0 = User, 1 = Owner, 2 = Admin.</summary>
        /// <example>1</example>
        [Required]
        public Role Role { get; set; }
    }
}
