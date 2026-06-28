using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    // DTO para capturar la dirección de una propiedad en los formularios.
    public class AddressDTO
    {
        [Required(ErrorMessage = "La ciudad debe ser obligatoria")]
        [MaxLength(100, ErrorMessage = "La ciudad no puede exceder los 100 caracteres.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia debe ser obligatorio")]
        [MaxLength(100, ErrorMessage = "La provincia no puede exceder los 100 caracteres.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "El país debe ser obligatorio")]
        [MaxLength(100, ErrorMessage = "El país no puede exceder los 100 caracteres.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección debe ser obligatoria")]
        [MaxLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código postal debe ser obligatorio")]
        [Range(1, 99999, ErrorMessage = "El código postal debe estar entre 1 y 99999.")]
        public int PostalCode { get; set; }
    }
}
