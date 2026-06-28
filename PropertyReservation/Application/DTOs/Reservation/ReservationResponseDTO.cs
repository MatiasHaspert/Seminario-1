namespace Application.DTOs.Reservation
{
    // Información completa de una reserva devuelta por la API.
    public class ReservationResponseDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
        public string PropertyImageUrl { get; set; } = string.Empty;
        public int GuestId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool HasReview { get; set; } = false;
    }
}
