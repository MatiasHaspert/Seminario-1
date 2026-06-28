using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payments
{
    /// <summary>Datos que envía el huésped al subir un comprobante de pago (multipart/form-data).</summary>
    public class PaymentRequestDTO
    {
        /// <summary>Identificador de la reserva a la que corresponde el pago.</summary>
        /// <example>1</example>
        [Required(ErrorMessage = "La reserva es obligatoria.")]
        public int reservationId { get; set; }

        /// <summary>Método de pago. 0 = CreditCard, 1 = DebitCard, 2 = PayPal, 3 = BankTransfer.</summary>
        /// <example>3</example>
        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>Archivo del comprobante de pago (imagen o PDF).</summary>
        [Required(ErrorMessage = "El comprobante es obligatorio.")]
        public IFormFile file { get; set; } = null!;
    }
}
