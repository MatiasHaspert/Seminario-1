using Domain.Enums;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Estadísticas del dashboard del admin resueltas en una única consulta a la BD.
    public class AdminStatsRepository
    {
        private readonly AppDbContext _context;

        public AdminStatsRepository(AppDbContext context)
        {
            _context = context;
        }

        // Combina todas las métricas en un solo SELECT mediante UNION ALL (Concat).
        // Cada rama es un GROUP BY que EF traduce a SQL; el conjunto viaja en un único
        // round-trip a la base de datos.
        public async Task<List<AdminStatRow>> GetStatsAsync()
        {
            var usersByRole = _context.Users
                .GroupBy(u => u.Role.ToString())
                .Select(g => new AdminStatRow
                {
                    Category = AdminStatCategories.UsersByRole,
                    Key = g.Key,
                    Value = g.Count()
                });

            var reservationsByStatus = _context.Reservations
                .GroupBy(r => r.Status.ToString())
                .Select(g => new AdminStatRow
                {
                    Category = AdminStatCategories.ReservationsByStatus,
                    Key = g.Key,
                    Value = g.Count()
                });

            // Properties respeta el filtro global de soft-delete (IsDeleted) igual que antes.
            var totalProperties = _context.Properties
                .GroupBy(p => 1)
                .Select(g => new AdminStatRow
                {
                    Category = AdminStatCategories.TotalProperties,
                    Key = "",
                    Value = g.Count()
                });

            var totalReviews = _context.Reviews
                .GroupBy(r => 1)
                .Select(g => new AdminStatRow
                {
                    Category = AdminStatCategories.TotalReviews,
                    Key = "",
                    Value = g.Count()
                });

            var pendingPayments = _context.Payment
                .Where(p => p.Status == PaymentStatus.UnderReview)
                .GroupBy(p => 1)
                .Select(g => new AdminStatRow
                {
                    Category = AdminStatCategories.PendingPayments,
                    Key = "",
                    Value = g.Count()
                });

            return await usersByRole
                .Concat(reservationsByStatus)
                .Concat(totalProperties)
                .Concat(totalReviews)
                .Concat(pendingPayments)
                .ToListAsync();
        }
    }
}
