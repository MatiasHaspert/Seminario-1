using Application.DTOs.PropertyImage;

namespace Application.DTOs.Property
{
    // Vista resumida de una propiedad para listados y resultados de búsqueda.
    public class PropertyListResponseDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal NightlyPrice { get; set; }

        public int MaxGuests { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        // Campos de dirección aplanados
        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string StreetAddress { get; set; } = string.Empty;

        public int PostalCode { get; set; }

        public decimal AverageRating { get; set; }

        public PropertyImageResponseDTO? MainImage { get; set; }

        public bool IsDeleted { get; set; }
    }
}
