namespace Domain.Enums
{
    // Estados por los que puede pasar un comprobante de pago.
    public enum PaymentStatus
    {
        UnderReview, // El comprobante fue subido y está esperando revisión del dueño
        Approved,    // El dueño validó el pago
        Rejected     // El dueño rechazó el pago
    }
}