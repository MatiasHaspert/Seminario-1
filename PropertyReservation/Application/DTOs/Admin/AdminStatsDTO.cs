namespace Application.DTOs.Admin
{
    // Datos agregados que se muestran en el dashboard del administrador.
    public class AdminStatsDTO
    {
        // Cantidad de usuarios por rol (User, Owner, Admin)
        public Dictionary<string, int> UsersByRole { get; set; } = new();

        public int TotalProperties { get; set; }
        public int TotalReviews { get; set; }

        // Cantidad de reservas agrupadas por estado
        public Dictionary<string, int> ReservationsByStatus { get; set; } = new();

        public int PendingPayments { get; set; }
    }
}
