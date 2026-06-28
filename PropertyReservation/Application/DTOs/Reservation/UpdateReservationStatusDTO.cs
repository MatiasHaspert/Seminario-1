using Domain.Enums;

namespace Application.DTOs.Reservation
{
    // Nuevo estado para una reserva (usado en actualizaciones administrativas).
    public class UpdateReservationStatusDTO
    {
        public ReservationStatus Status { get; set; }
    }
}
