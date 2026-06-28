using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payments
{
    /// <summary>Nuevo estado que el dueño quiere asignar a un pago.</summary>
    public class ChangePaymentStatusDTO
    {
        /// <summary>Estado destino del comprobante. 1 = Approved (aprobado), 2 = Rejected (rechazado).</summary>
        /// <example>1</example>
        public PaymentStatus PaymentStatus { get; set; }
    }
}
