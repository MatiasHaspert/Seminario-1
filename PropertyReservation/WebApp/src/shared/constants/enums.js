// Enumeraciones compartidas que reflejan los enums del backend.
// Se usan en lugar de strings "sueltos" para evitar errores de tipeo y centralizar
// los valores válidos. Object.freeze() los hace inmutables (no se pueden modificar).

// Estados por los que pasa una reserva a lo largo de su ciclo de vida.
export const ReservationStatus = Object.freeze({
    PendingPayment: "PendingPayment",   // Creada, esperando que el huésped suba el comprobante
    PaymentUploaded: "PaymentUploaded", // Comprobante subido, pendiente de revisión del dueño
    Confirmed: "Confirmed",             // Pago aprobado por el dueño
    Cancelled: "Cancelled",             // Cancelada por el huésped
    Completed: "Completed",             // Estadía finalizada
});

// Estado de revisión de un pago (comprobante de transferencia).
export const PaymentStatus = Object.freeze({
    Pending: "Pending",   // Subido, a la espera de revisión
    Approved: "Approved", // Aceptado por el dueño
    Rejected: "Rejected", // Rechazado por el dueño
});

// Forma de pago elegida por el huésped.
export const PaymentMethod = Object.freeze({
    Transfer: "Transfer",
    Cash: "Cash",
});

// Roles de usuario; determinan qué rutas y acciones tiene permitidas cada uno.
export const Role = Object.freeze({
    User: "User",
    Owner: "Owner",
    Admin: "Admin",
});
