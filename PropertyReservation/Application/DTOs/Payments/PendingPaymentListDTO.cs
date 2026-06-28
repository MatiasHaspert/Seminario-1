using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payments
{
    // Información resumida de un pago pendiente de revisión por el dueño.
    public class PendingPaymentListDTO
    {
        public Guid PaymentId { get; set; }

        public int ReservationId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public DateOnly ReservationStart { get; set; }
        public DateOnly ReservationEnd { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
