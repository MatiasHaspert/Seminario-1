using Domain.Entities;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.Property
{
    /// <summary>Datos que envía el dueño al crear o editar una propiedad.</summary>
    public class PropertyRequestDTO
    {
        /// <summary>Título de la publicación.</summary>
        /// <example>Casa Alpina</example>
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>Precio por noche.</summary>
        /// <example>120000</example>
        [Required(ErrorMessage = "El precio por noche es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio por noche debe ser mayor que cero.")]
        public decimal NightlyPrice { get; set; }

        /// <summary>Capacidad máxima de huéspedes.</summary>
        /// <example>4</example>
        [Required(ErrorMessage = "La capacidad de huéspedes es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad de huéspedes debe ser al menos 1.")]
        public int MaxGuests { get; set; }

        /// <summary>Cantidad de habitaciones.</summary>
        /// <example>3</example>
        [Required(ErrorMessage = "La cantidad de habitaciones es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de habitaciones debe ser un número positivio.")]
        public int Bedrooms { get; set; }

        /// <summary>Cantidad de baños.</summary>
        /// <example>2</example>
        [Required(ErrorMessage = "La cantidad de baños es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de baños debe ser un número positivio.")]
        public int Bathrooms { get; set; }

        // Campos de dirección aplanados
        /// <summary>Ciudad de la propiedad.</summary>
        /// <example>Rosario</example>
        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [MaxLength(100, ErrorMessage = "La ciudad no puede exceder los 100 caracteres.")]
        public string City { get; set; } = string.Empty;

        /// <summary>Provincia o estado.</summary>
        /// <example>Santa Fe</example>
        [Required(ErrorMessage = "La provincia es obligatoria.")]
        [MaxLength(100, ErrorMessage = "La provincia no puede exceder los 100 caracteres.")]
        public string State { get; set; } = string.Empty;

        /// <summary>País.</summary>
        /// <example>Argentina</example>
        [Required(ErrorMessage = "El país es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El país no puede exceder los 100 caracteres.")]
        public string Country { get; set; } = string.Empty;

        /// <summary>Dirección (calle y número).</summary>
        /// <example>Calle Montaña 123</example>
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [MaxLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>Código postal (entre 1 y 99999).</summary>
        /// <example>2000</example>
        [Required(ErrorMessage = "El código postal es obligatorio.")]
        [Range(1, 99999, ErrorMessage = "El código postal debe estar entre 1 y 99999.")]
        public int PostalCode { get; set; }

        /// <summary>Descripción de la propiedad (hasta 3000 caracteres).</summary>
        /// <example>Encantadora casa de montaña, ideal para familias, con amplio jardín y entorno tranquilo.</example>
        [MinLength(0, ErrorMessage = "La descripción debe tener al menos 0 caracteres.")]
        [MaxLength(3000, ErrorMessage = "La descripción no puede exceder los 3000 caracteres.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>IDs de los servicios (amenities) a asociar, elegidos de la lista existente.</summary>
        /// <example>[1, 2]</example>
        public ICollection<int> AmenityIds { get; set; } = new List<int>();
    }
}
