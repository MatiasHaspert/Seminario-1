using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    // Propiedad publicada por un dueño; contiene toda la información de alojamiento y su lógica de negocio.
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Precision(18, 2)]
        public decimal NightlyPrice { get; set; }

        [Required]
        public int MaxGuests { get; set; }

        [Required]
        public int Bedrooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        [Required]
        public Address? Address { get; set; }

        public string Description { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        public User Owner { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();

        public ICollection<PropertyAvailability> Availabilities { get; set; } = new List<PropertyAvailability>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Verifica si la propiedad tiene disponibilidad que cubra el rango de fechas pedido.
        public bool IsAvailableForDateRange(DateOnly startDate, DateOnly endDate)
        {
            return Availabilities.Any(a =>
                a.StartDate <= startDate &&
                a.EndDate >= endDate
            );
        }

        // Verifica si hay alguna reserva activa que se solape con el rango indicado.
        public bool HasConflictingReservation(DateOnly startDate, DateOnly endDate, int? excludeReservationId = null)
        {
            return Reservations.Any(r =>
                (r.Status == ReservationStatus.PendingPayment ||
                 r.Status == ReservationStatus.PaymentUploaded ||
                 r.Status == ReservationStatus.Confirmed) &&
                (excludeReservationId == null || r.Id != excludeReservationId) &&
                startDate < r.EndDate &&
                endDate > r.StartDate
            );
        }

        // Calcula el precio total multiplicando las noches por el precio por noche.
        public decimal CalculateTotalPrice(DateOnly startDate, DateOnly endDate)
        {
            if (endDate <= startDate)
                throw new InvalidOperationException("La fecha de salida debe ser posterior al check-in.");

            var nights = endDate.DayNumber - startDate.DayNumber;

            return nights * NightlyPrice;

        }
    }
}
