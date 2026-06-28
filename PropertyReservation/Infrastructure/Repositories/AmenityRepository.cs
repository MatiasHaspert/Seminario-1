using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Acceso a datos para los servicios/amenities disponibles en el sistema.
    public class AmenityRepository
    {
        private readonly AppDbContext _context;

        public AmenityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Amenity>> GetAllAsync()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task<Amenity?> GetByIdAsync(int id)
        {
            return await _context.Amenities.FindAsync(id);
        }

        public async Task<Amenity> AddAsync(Amenity amenity)
        {
            await _context.Amenities.AddAsync(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        public async Task UpdateAsync(Amenity amenity)
        {
            _context.Amenities.Update(amenity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Amenity amenity)
        {
            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Amenities.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _context.Amenities.AnyAsync(a =>
                a.Name.ToLower() == name.ToLower() &&
                (excludeId == null || a.Id != excludeId));
        }

        public async Task<bool> IsAssignedToAnyPropertyAsync(int id)
        {
            return await _context.Amenities
                .Where(a => a.Id == id)
                .AnyAsync(a => a.Properties.Any());
        }

        public async Task<List<Amenity>> GetByIdsAsync(ICollection<int> ids)
        {
            return await _context.Amenities
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }
    }
}
