using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payments
{
    // Confirmación devuelta después de registrar un pago.
    public class PaymentResponseDTO
    {
        public Guid paymentId { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
