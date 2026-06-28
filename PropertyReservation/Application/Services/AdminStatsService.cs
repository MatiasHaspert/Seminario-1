using Application.DTOs.Admin;
using Infrastructure.Repositories;

namespace Application.Services
{
    // Recopila las estadísticas generales que se muestran en el dashboard del administrador.
    public class AdminStatsService
    {
        private readonly AdminStatsRepository _adminStatsRepository;

        public AdminStatsService(AdminStatsRepository adminStatsRepository)
        {
            _adminStatsRepository = adminStatsRepository;
        }

        public async Task<AdminStatsDTO> GetStatsAsync()
        {
            // Una sola consulta a la BD; acá reagrupamos las filas (Categoría/Clave/Valor) en el DTO.
            var rows = await _adminStatsRepository.GetStatsAsync();

            return new AdminStatsDTO
            {
                UsersByRole = rows
                    .Where(r => r.Category == AdminStatCategories.UsersByRole)
                    .ToDictionary(r => r.Key, r => r.Value),
                ReservationsByStatus = rows
                    .Where(r => r.Category == AdminStatCategories.ReservationsByStatus)
                    .ToDictionary(r => r.Key, r => r.Value),
                TotalProperties = Scalar(rows, AdminStatCategories.TotalProperties),
                TotalReviews = Scalar(rows, AdminStatCategories.TotalReviews),
                PendingPayments = Scalar(rows, AdminStatCategories.PendingPayments),
            };

            // Métricas escalares: si la tabla está vacía no hay fila, por lo que el total es 0.
            static int Scalar(List<AdminStatRow> rows, string category)
                => rows.FirstOrDefault(r => r.Category == category)?.Value ?? 0;
        }
    }
}
