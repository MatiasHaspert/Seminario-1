using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Amenity
{
    /// <summary>Datos requeridos para crear o actualizar un servicio.</summary>
    public class AmenityRequestDTO
    {
        /// <summary>Nombre del servicio (entre 2 y 100 caracteres).</summary>
        /// <example>Wi-Fi</example>
        [Required(ErrorMessage = "El nombre del servicio es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del servicio no puede exceder los 100 caracteres")]
        [MinLength(2, ErrorMessage = "El nombre del servicio debe tener al menos 2 caracteres")]
        public string Name { get; set; } = string.Empty;
    }
}
