namespace Application.DTOs.PropertyAvailability
{
    // Rango de disponibilidad que se muestra al público, sin datos internos.
    public class PropertyAvailabilityPublicDTO
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
