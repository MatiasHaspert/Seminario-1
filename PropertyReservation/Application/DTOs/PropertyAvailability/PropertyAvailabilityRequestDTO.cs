using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PropertyAvailability
{
    /// <summary>Datos requeridos para crear o actualizar un rango de disponibilidad.</summary>
    public class PropertyAvailabilityRequestDTO
    {
        /// <summary>Fecha de inicio del rango disponible.</summary>
        /// <example>2026-01-01</example>
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateOnly StartDate { get; set; }

        /// <summary>Fecha de fin del rango disponible.</summary>
        /// <example>2026-12-31</example>
        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateOnly EndDate { get; set; }

        /// <summary>Identificador de la propiedad.</summary>
        /// <example>1</example>
        [Required(ErrorMessage = "El ID de la propiedad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la propiedad debe ser válido.")]
        public int PropertyId { get; set; }
    }
}
