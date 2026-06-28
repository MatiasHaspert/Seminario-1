using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Acceso a datos para reservas, incluyendo consultas de estado y estadísticas.
    public class ReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<List<Reservation>> GetByUserIdOrderByDateAsync(int userId)
        {
            return await _context.Reservations
                                 .Include(r => r.Property)
                                    .ThenInclude(p => p.Images)
                                 .Where(r => r.GuestId == userId)
                                 .OrderByDescending(r => r.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<List<Reservation>> GetByPropertyIdForOwnerIdAsync(int propertyId, int ownerId)
        {
            return await _context.Reservations
                                 .Include(r => r.Guest)
                                 .Include(r => r.Property)
                                    .ThenInclude(p => p.Images)
                                 .Where(r => r.PropertyId == propertyId && r.Property.OwnerId == ownerId)
                                 .ToListAsync();
        }

        public async Task<Reservation?> GetByIdWithPropertyAsync(int reservationId)
        {
            return await _context.Reservations
                                 .Include(r => r.Guest)
                                 .Include(r => r.Property)
                                    .ThenInclude(p => p.Images)
                                 .FirstOrDefaultAsync(r => r.Id == reservationId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Reservations
                                 .Include(r => r.Guest)
                                 .Include(r => r.Property)
                                    .ThenInclude(p => p.Images)
                                 .Where(r => r.Property.OwnerId == ownerId)
                                 .ToListAsync();
        }

        // Listado global para Administrador con filtros opcionales (estado, propiedad, huésped y rango de fechas).
        public async Task<List<Reservation>> GetAllWithFiltersAsync(
            ReservationStatus? status,
            int? propertyId,
            int? guestId,
            DateOnly? from,
            DateOnly? to)
        {
            var query = _context.Reservations
                                .Include(r => r.Guest)
                                .Include(r => r.Property)
                                    .ThenInclude(p => p.Images)
                                .AsQueryable();

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (propertyId.HasValue)
                query = query.Where(r => r.PropertyId == propertyId.Value);

            if (guestId.HasValue)
                query = query.Where(r => r.GuestId == guestId.Value);

            // Filtro por solapamiento con el rango [from, to]: la reserva se incluye si intersecta la ventana pedida.
            if (from.HasValue)
                query = query.Where(r => r.EndDate >= from.Value);

            if (to.HasValue)
                query = query.Where(r => r.StartDate <= to.Value);

            return await query
                         .OrderByDescending(r => r.CreatedAt)
                         .ToListAsync();
        }
    }
}
