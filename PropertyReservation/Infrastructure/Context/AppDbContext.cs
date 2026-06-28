using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Context
{
    // Contexto principal de Entity Framework; expone los DbSets y configura el modelo.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<PropertyAvailability> PropertyAvailabilities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<Property>().OwnsOne(p => p.Address);
            modelBuilder.Entity<User>().OwnsOne(u => u.Address);

            // DateOnly no tiene soporte nativo en SQL Server, se convierte a DateTime
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateOnly) || p.ClrType == typeof(DateOnly?));

                foreach (var property in properties)
                {
                    // Conversión de DateOnly no nullable
                    if (property.ClrType == typeof(DateOnly))
                    {
                        property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateOnly, DateTime>(
                            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
                            dateTime => DateOnly.FromDateTime(dateTime)
                        ));
                    }
                    else if (property.ClrType == typeof(DateOnly?))
                    {
                        // Conversión de DateOnly nullable
                        property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateOnly?, DateTime?>(
                            dateOnly => dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : null,
                            dateTime => dateTime.HasValue ? DateOnly.FromDateTime(dateTime.Value) : null
                        ));
                    }
                }
            }

            // Las propiedades con soft-delete no aparecen en ninguna consulta por defecto
            modelBuilder.Entity<Property>()
                .HasQueryFilter(p => !p.IsDeleted);

            // Solo puede haber una imagen marcada como principal por propiedad
            modelBuilder.Entity<PropertyImage>()
                .HasIndex(i => new { i.PropertyId, i.IsMainImage })
                .IsUnique()
                .HasFilter("[IsMainImage] = 1");

            // Restrict es obligatorio aquí: Property.OwnerId -> User está en Cascade, lo que crea una
            // segunda ruta de borrado User -> Property -> Review/Reservation además de la directa
            // User -> Review/Reservation. SQL Server prohíbe rutas de cascada múltiples a la misma tabla
            // (error 1785); sin este Restrict, EF usaría Cascade por defecto y la migración fallaría al aplicarse.
            modelBuilder.Entity<Review>()
                        .HasOne(r => r.User)
                        .WithMany(u => u.Reviews)
                        .HasForeignKey(r => r.UserId)
                        .OnDelete(DeleteBehavior.Restrict);

            // Mismo motivo para reservas: corta la ruta de cascada redundante hacia Reservation vía GuestId.
            modelBuilder.Entity<Reservation>()
                        .HasOne(res => res.Guest)
                        .WithMany(u => u.Reservations)
                        .HasForeignKey(res => res.GuestId)
                        .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            // Servicios precargados para que la app tenga datos desde la primera migración
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "Wi-Fi" },
                new Amenity { Id = 2, Name = "Aire acondicionado" },
                new Amenity { Id = 3, Name = "Calefacción" },
                new Amenity { Id = 4, Name = "Televisor" },
                new Amenity { Id = 5, Name = "Cocina equipada" },
                new Amenity { Id = 6, Name = "Heladera" },
                new Amenity { Id = 7, Name = "Microondas" },
                new Amenity { Id = 8, Name = "Lavarropas" },
                new Amenity { Id = 9, Name = "Piscina" },
                new Amenity { Id = 10, Name = "Estacionamiento gratuito" },
                new Amenity { Id = 11, Name = "Gimnasio" },
                new Amenity { Id = 12, Name = "Jacuzzi" },
                new Amenity { Id = 13, Name = "Balcón o terraza" },
                new Amenity { Id = 14, Name = "Vista al mar" },
                new Amenity { Id = 15, Name = "Parrilla" },
                new Amenity { Id = 16, Name = "Mascotas permitidas" },
                new Amenity { Id = 17, Name = "Desayuno incluido" },
                new Amenity { Id = 18, Name = "Acceso a playa" },
                new Amenity { Id = 19, Name = "Caja fuerte" },
                new Amenity { Id = 20, Name = "Cámaras de seguridad" }
            );

        }

    }
}
