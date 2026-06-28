namespace Application.DTOs.PropertyAvailability
{
    // Disponibilidad de una propiedad tal como se devuelve en la respuesta de la API.
    public class PropertyAvailabilityResponseDTO
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int PropertyId { get; set; }
    }
}
