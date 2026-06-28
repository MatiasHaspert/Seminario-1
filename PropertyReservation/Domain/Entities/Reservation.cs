using Domain.Enums;

namespace Domain.Entities
{
    // Reserva de una propiedad. Implementa la máquina de estados del ciclo de vida de una reserva.
    public class Reservation
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public int GuestId { get; set; }
        public User Guest { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int TotalGuests { get; set; }
        // Precio total calculado al momento de crear la reserva
        public decimal TotalPrice { get; set; }
        // Estado actual dentro del ciclo de vida de la reserva
        public ReservationStatus Status { get; set; } = ReservationStatus.PendingPayment;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public bool IsPaid => Payments.Any(p => p.Status == PaymentStatus.Approved);

        // Una reserva está activa mientras siga vigente en el ciclo de vida (no fue cancelada ni completada).
        public bool IsActive =>
            Status == ReservationStatus.PendingPayment ||
            Status == ReservationStatus.PaymentUploaded ||
            Status == ReservationStatus.Confirmed;

        // Lanza excepción si el estado actual no es uno de los permitidos para la transición.
        private void EnsureStatus(params ReservationStatus[] allowed)
        {
            if (!allowed.Contains(Status))
                throw new InvalidOperationException(
                    $"Acción inválida desde estado {Status} de la reserva.");
        }

        // El huésped sube el comprobante de pago.
        public void UploadPayment()
        {
            EnsureStatus(ReservationStatus.PendingPayment);

            Status = ReservationStatus.PaymentUploaded;
        }

        // El dueño valida el comprobante y confirma la reserva.
        public void ConfirmPayment()
        {
            EnsureStatus(ReservationStatus.PaymentUploaded);

            Status = ReservationStatus.Confirmed;
        }

        // La estadía finalizó y el dueño cierra la reserva.
        public void Completed(DateOnly today)
        {
            EnsureStatus(ReservationStatus.Confirmed);

            if (today < EndDate)
                throw new InvalidOperationException(
                    "No se puede completar la reserva antes de la fecha de check-out.");

            Status = ReservationStatus.Completed;
        }

        // El huésped o el dueño cancela la reserva.
        public void Cancel()
        {
            EnsureStatus(
                ReservationStatus.PendingPayment,
                ReservationStatus.PaymentUploaded,
                ReservationStatus.Confirmed
            );

            Status = ReservationStatus.Cancelled;
        }

        // Revierte la reserva a PendingPayment cuando el pago fue rechazado.
        public void ReturnToPendingPaymentAfterRejected()
        {
            EnsureStatus(ReservationStatus.PaymentUploaded);
            Status = ReservationStatus.PendingPayment;
        }
    }
}
