namespace Application.DTOs.Reservation
{
    // Rango de fechas de una reserva que se muestra en el calendario público de la propiedad.
    public class ReservationPublicDTO
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
