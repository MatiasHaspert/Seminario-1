namespace Infrastructure.Repositories
{
    // Una fila del resultado agregado del dashboard, en formato Categoría/Clave/Valor.
    // Las métricas agrupadas (usuarios por rol, reservas por estado) aportan una fila por
    // grupo; las métricas escalares (totales) aportan una única fila con Key vacío.
    public class AdminStatRow
    {
        public string Category { get; set; } = "";
        public string Key { get; set; } = "";
        public int Value { get; set; }
    }

    // Nombres de categoría compartidos entre el repositorio (arma las filas) y el
    // servicio (las reagrupa en el DTO). Viven en Infrastructure porque Application
    // ya referencia a Infrastructure (no al revés).
    public static class AdminStatCategories
    {
        public const string UsersByRole = "UsersByRole";
        public const string ReservationsByStatus = "ReservationsByStatus";
        public const string TotalProperties = "TotalProperties";
        public const string TotalReviews = "TotalReviews";
        public const string PendingPayments = "PendingPayments";
    }
}
