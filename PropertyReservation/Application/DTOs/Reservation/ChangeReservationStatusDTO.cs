using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Reservation
{
    /// <summary>Nuevo estado que el dueño quiere asignar a una reserva.</summary>
    public class ChangeReservationStatusDTO
    {
        /// <summary>Estado destino de la reserva (ver valores posibles en el esquema).</summary>
        /// <example>2</example>
        public ReservationStatus Status { get; set; }
    }
}
