namespace Application.DTOs
{
    /// <summary>Filtros opcionales que el usuario puede aplicar al buscar propiedades.</summary>
    public class PropertySearchRequestDTO
    {
        /// <summary>Ciudad donde buscar.</summary>
        /// <example>Rosario</example>
        public string? City { get; set; }

        /// <summary>Cantidad mínima de huéspedes que debe admitir la propiedad.</summary>
        /// <example>2</example>
        public int? Guests { get; set; }

        /// <summary>Fecha de check-in deseada.</summary>
        /// <example>2026-08-05</example>
        public DateOnly? CheckIn { get; set; }

        /// <summary>Fecha de check-out deseada.</summary>
        /// <example>2026-08-10</example>
        public DateOnly? CheckOut { get; set; }
    }
}
