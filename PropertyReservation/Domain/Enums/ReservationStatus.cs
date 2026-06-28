namespace Domain.Enums
{
    // Ciclo de vida de una reserva, desde que se crea hasta que se completa o cancela.
    public enum ReservationStatus
    {
        PendingPayment = 0,   // El huésped eligió fechas, falta subir el comprobante
        PaymentUploaded = 1,  // El huésped subió el comprobante
        Confirmed = 2,        // El dueño validó el pago
        // 3 (Rejected) se removió: rechazar un pago devuelve la reserva a PendingPayment.
        Cancelled = 4,        // Cancelación manual
        Completed = 5         // Estadía finalizada
    }
}
