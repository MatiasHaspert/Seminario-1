using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Acceso a datos para los comprobantes de pago y sus estados.
    public class PaymentsRepository
    {
        private readonly AppDbContext _context;

        public PaymentsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment?> GetByIdWithReservationAndPropertyAsync(Guid paymentId)
        {
            return await _context.Payment
                                 .Include(p => p.Reservation)
                                 .ThenInclude(r => r.Property)
                                 .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<Payment?> GetByIdWithReservationAsync(Guid paymentId)
        {
            return await _context.Payment
                                 .Include(p => p.Reservation)
                                 .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<List<Payment>> GetUnderReviewPaymentsByOwnerIdAsync(int ownerId)
        {
            return await _context.Payment
                .Where(p => p.Status == PaymentStatus.UnderReview)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Property)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Guest)
                .Where(p => p.Reservation.Property.OwnerId == ownerId)
                .ToListAsync();
        }

        // Todos los pagos bajo revisión, sin filtrar por owner (uso del Admin).
        public async Task<List<Payment>> GetAllUnderReviewPaymentsAsync()
        {
            return await _context.Payment
                .Where(p => p.Status == PaymentStatus.UnderReview)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Property)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Guest)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Payment payment)
        {
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasApprovedOrUnderReviewPaymentAsync(int reservationId)
        {
            return await _context.Payment
                .AnyAsync(p => p.ReservationId == reservationId &&
                               (p.Status == PaymentStatus.Approved || p.Status == PaymentStatus.UnderReview));
        }
    }
}
