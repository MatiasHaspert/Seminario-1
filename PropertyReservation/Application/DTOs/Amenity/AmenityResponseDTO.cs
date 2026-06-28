namespace Application.DTOs.Amenity
{
    // Representa un servicio tal como se devuelve en la respuesta de la API.
    public class AmenityResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
