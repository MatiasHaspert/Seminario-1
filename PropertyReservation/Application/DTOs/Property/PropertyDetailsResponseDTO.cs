using Application.DTOs.Amenity;
using Application.DTOs.PropertyAvailability;
using Application.DTOs.PropertyImage;
using Application.DTOs.Reservation;
using Application.DTOs.Review;

namespace Application.DTOs.Property
{
    // Vista completa de una propiedad, incluyendo imágenes, reseñas, servicios y disponibilidad.
    public class PropertyDetailsResponseDTO
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

        public string Description { get; set; } = string.Empty;

        public decimal AverageRating { get; set; }

        public List<AmenityResponseDTO> Amenities { get; set; } = new List<AmenityResponseDTO>();

        public List<PropertyImageResponseDTO> Images { get; set; } = new List<PropertyImageResponseDTO>();

        public List<ReviewResponseDTO> Reviews { get; set; } = new List<ReviewResponseDTO>();

        public List<PropertyAvailabilityPublicDTO> AvailableRanges { get; set; } = new List<PropertyAvailabilityPublicDTO>();

        public List<ReservationPublicDTO> ReservedRanges { get; set; } = new List<ReservationPublicDTO>();
    }
}
