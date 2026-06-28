using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Review
{
    /// <summary>Datos que envía el usuario al crear o editar una reseña.</summary>
    public class ReviewRequestDTO
    {
        /// <summary>Calificación de la propiedad, del 1 al 5.</summary>
        /// <example>5</example>
        [Required(ErrorMessage = "La calificación es requerida")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int Rating { get; set; } // Del 1 al 5

        /// <summary>Comentario de la reseña (entre 10 y 1000 caracteres).</summary>
        /// <example>Excelente lugar, muy limpio y con una vista increíble.</example>
        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres")]
        [MinLength(10, ErrorMessage = "El comentario debe tener al menos 10 caracteres")]
        public string Comment { get; set; } = string.Empty;

        /// <summary>Identificador de la propiedad reseñada.</summary>
        /// <example>1</example>
        [Required(ErrorMessage = "El ID de la propiedad es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la propiedad debe ser un número válido")]
        public int PropertyId { get; set; }
    }
}
