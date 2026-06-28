using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Acceso a datos para los rangos de disponibilidad de propiedades.
    public class PropertyAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public PropertyAvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PropertyAvailability>> GetByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyAvailabilities
                .Where(pa => pa.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<PropertyAvailability?> GetByIdWithPropertyAndReservationsAsync(int id)
        {
            return await _context.PropertyAvailabilities
                .Include(a => a.Property)
                    .ThenInclude(p => p.Reservations)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PropertyAvailability> AddAsync(PropertyAvailability availability)
        {
            await _context.PropertyAvailabilities.AddAsync(availability);
            await _context.SaveChangesAsync();
            return availability;
        }

        public async Task UpdateAsync(PropertyAvailability availability)
        {
            _context.Update(availability);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PropertyAvailability availability)
        {
            _context.PropertyAvailabilities.Remove(availability);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PropertyAvailabilities.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> HasOverlapAsync(int propertyId, DateOnly startDate, DateOnly endDate, int? excludeId = null)
        {
            var query = _context.PropertyAvailabilities
                .Where(a => a.PropertyId == propertyId &&
                           a.StartDate <= endDate &&
                           a.EndDate >= startDate);

            // Si se está editando una disponibilidad existente, la excluimos de la comparación
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
