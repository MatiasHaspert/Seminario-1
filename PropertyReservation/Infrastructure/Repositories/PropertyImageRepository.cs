using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Acceso a datos para las imágenes de propiedades.
    public class PropertyImageRepository
    {
        private readonly AppDbContext _context;

        public PropertyImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(int propertyId)
        {
            return await _context.PropertyImages
                .Where(img => img.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<PropertyImage?> GetByIdWithPropertyAsync(int imageId)
        {
            return await _context.PropertyImages
                .Include(pi => pi.Property)
                .FirstOrDefaultAsync(pi => pi.Id == imageId);
        }

        public async Task AddRangeAsync(List<PropertyImage> images)
        {
            await _context.PropertyImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PropertyImage image)
        {
            _context.PropertyImages.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PropertyImage image)
        {
            _context.PropertyImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasMainImageAsync(int propertyId)
        {
            return await _context.PropertyImages
                .AnyAsync(img => img.PropertyId == propertyId && img.IsMainImage);
        }

    }
}
