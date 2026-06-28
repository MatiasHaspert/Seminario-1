using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Acceso a datos para las reseñas de propiedades.
    public class ReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<IEnumerable<Review>> GetByPropertyIdAsync(int propertyId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Property)
                .Where(r => r.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllOrderedByDateAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Property)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<Review?> FindByIdAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        public async Task<Review?> GetByUserAndPropertyAsync(int userId, int propertyId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.PropertyId == propertyId);
        }

        // Verifica si el usuario tiene al menos una reserva completada en la propiedad.
        public async Task<bool> HasUserCompletedReservationAsync(int userId, int propertyId)
        {
            return await _context.Reservations
                .AnyAsync(reservation =>
                    reservation.GuestId == userId &&
                    reservation.PropertyId == propertyId &&
                    reservation.EndDate < DateOnly.FromDateTime(DateTime.UtcNow));
        }

        public async Task<bool> ExistsByUserIdAndPropertyIdAsync(int userId, int propertyId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.PropertyId == propertyId);
        }
    }
}
