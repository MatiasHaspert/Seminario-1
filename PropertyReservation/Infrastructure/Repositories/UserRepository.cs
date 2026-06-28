using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Infrastructure.Repositories
{
    // Acceso a datos para usuarios, con soporte de filtros y agrupaciones.
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsWithEmailAsync(string email, int? excludeUserId = null)
        {
            if (excludeUserId.HasValue)
            {
                return await _context.Users
                    .AnyAsync(u => u.Email == email && u.Id != excludeUserId.Value);
            }

            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetByEmailOrRolOrActive(string? emailFilter, string? roleFilter, bool? isActiveFilter)
        {
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrWhiteSpace(emailFilter))
            {
                // StartsWith equivale a un LIKE 'filtro%' en SQL
                query = query.Where(u => u.Email.StartsWith(emailFilter));
            }

            // Convertimos el string a enum antes de comparar para evitar problemas con EF
            if (!string.IsNullOrWhiteSpace(roleFilter) && Enum.TryParse(roleFilter, true, out Role parsedRole))
            {
                query = query.Where(u => u.Role == parsedRole);
            }

            if (isActiveFilter.HasValue)
            {
                query = query.Where(u => u.IsActive == isActiveFilter.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<User?> GetByIdWithDetails(int userId)
        {
            return await _context.Users
                .Include(u => u.Properties)
                .Include(u => u.Reservations)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
