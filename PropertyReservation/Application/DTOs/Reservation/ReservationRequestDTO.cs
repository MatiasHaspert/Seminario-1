namespace Application.DTOs.Reservation
{
    /// <summary>Datos que envía el huésped al crear una reserva.</summary>
    public class ReservationRequestDTO
    {
        /// <summary>Identificador de la propiedad a reservar.</summary>
        /// <example>1</example>
        public int PropertyId { get; set; }

        /// <summary>Fecha de inicio de la estadía (check-in).</summary>
        /// <example>2026-08-05</example>
        public DateOnly StartDate { get; set; }

        /// <summary>Fecha de fin de la estadía (check-out).</summary>
        /// <example>2026-08-10</example>
        public DateOnly EndDate { get; set; }

        /// <summary>Cantidad total de huéspedes.</summary>
        /// <example>2</example>
        public int TotalGuests { get; set; }
    }
}
